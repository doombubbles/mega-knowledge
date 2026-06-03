using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
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

        var supeFilter = FilterInBaseTowerIdModel.Create(new()
        {
            name = "Filter_Supes",
            baseIds = new[] { TowerType.SuperMonkey }
        });

        var tier = MegaKnowledgeMod.OpKnowledge ? model.tier : model.tier - 2;

        var buffs = new[]
        {
            RateSupportModel.Create(new()
            {
                multiplier = 1f - tier * .05f,
                isUnique = true,
                mutatorId = "TheBigThree1"
            }).ApplyBuffIcon<SunAvatarIcon>(),
            RangeSupportModel.Create(new()
            {
                isUnique = true,
                multiplier = tier * .05f,
                mutatorId = "TheBigThree2"
            }).ApplyBuffIcon<RoboMonkeyIcon>(),
            PierceSupportModel.Create(new()
            {
                isUnique = true,
                pierce = model.tier,
                mutatorId = "TheBigThree3"
            }).ApplyBuffIcon<DarkKnightIcon>()
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
                    FilterInTowerTiersModel.Create(new()
                    {
                        path1MinTier = otherPath == 0 ? 3 : 0,
                        path1MaxTier = 5,
                        path2MinTier = otherPath == 1 ? 3 : 0,
                        path2MaxTier = 5,
                        path3MinTier = otherPath == 2 ? 3 : 0,
                        path3MaxTier = 5
                    })
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