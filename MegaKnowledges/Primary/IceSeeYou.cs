using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Collections.Generic;

namespace MegaKnowledge.MegaKnowledges.Primary;

public class IceSeeYou : MegaKnowledge
{
    public override string TowerId => TowerType.IceMonkey;
    public override string Description => "Ice Monkeys detect and remove Camo from Bloons.";
    public override int Offset => 400;

    public override void Apply(TowerModel model)
    {
        var behavior = new RemoveBloonModifiersModel("RemoveBloonModifiersModel_", false, true, false, false, false,
            new Il2CppStringArray(0), new Il2CppStringArray(0));
        foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
        {
            projectileModel.AddBehavior(behavior.Duplicate());
        }

        model.GetDescendants<FilterInvisibleModel>().ForEach(invisibleModel => invisibleModel.isActive = false);
    }
}