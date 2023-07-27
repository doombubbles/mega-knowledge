using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Utils;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Magic;

public class TheBigThree : MegaKnowledge
{
    public override string TowerId => TowerType.SuperMonkey;

    public override string Description =>
        "Sun Avatars, Robo Cops, and Dark Knights buff the attack speed / range / pierce of any of the other two types nearby them.";

    public override int Offset => -1600;

    public override void Apply(TowerModel model)
    {
        if (model.tier < 3) return;

        var supeFilter = new FilterInBaseTowerIdModel("Filter_Supes", new[] { TowerType.SuperMonkey });

        var tier = MegaKnowledgeMod.OpKnowledge ? model.tier : model.tier - 2;

        var buffs = new[]
        {
            new RateSupportModel("", 1f - tier * .05f, true, "TheBigThree1", false, 0, null, null, null)
                .ApplyBuffIcon<SunAvatarIcon>(),
            new RangeSupportModel("", true, tier * .05f, 0f, "TheBigThree2", null, false, null, null)
                .ApplyBuffIcon<RoboMonkeyIcon>(),
            new PierceSupportModel("", true, model.tier, "TheBigThree3", null, false, null, null)
                .ApplyBuffIcon<DarkKnightIcon>()
        };

        for (var path = 0; path < 3; path++)
        {
            if (model.tiers[path] < 3) continue;

            var buff = buffs[path];
            for (var otherPath = 0; otherPath < 3; otherPath++)
            {
                if (path == otherPath) continue;

                var buffForPath = buff.Duplicate();
                buffForPath.name = buff.GetIl2CppType().Name + "_" + otherPath;
                buffForPath.filters = new TowerFilterModel[]
                {
                    supeFilter.Duplicate(),
                    new FilterInTowerTiersModel("",
                        otherPath == 0 ? 3 : 0, 5,
                        otherPath == 1 ? 3 : 0, 5,
                        otherPath == 2 ? 3 : 0, 5)
                };
                model.AddBehavior(buffForPath);
            }
        }
    }
}

public class SunAvatarIcon : ModBuffIcon
{
    public override string Icon => VanillaSprites.SunAvatarUpgradeIcon;
}

public class RoboMonkeyIcon : ModBuffIcon
{
    public override string Icon => VanillaSprites.RoboMonkeyUpgradeIcon;
}

public class DarkKnightIcon : ModBuffIcon
{
    public override string Icon => VanillaSprites.DarkKnightUpgradeIcon;
}