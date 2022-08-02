using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using Il2CppSystem.Collections.Generic;

namespace MegaKnowledge.MegaKnowledges.Primary
{
    public class IceSeeYou : MegaKnowledge
    {
        public override string TowerId => TowerType.IceMonkey;
        public override string Description => "Ice Monkeys detect and remove Camo from Bloons.";
        public override int Offset => 400;

        public override void Apply(TowerModel model)
        {
            var behavior = new RemoveBloonModifiersModel("RemoveBloonModifiersModel_", false, true, false, false, false,
                new List<string>());
            foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
            {
                projectileModel.AddBehavior(behavior.Duplicate());
            }

            model.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
        }
    }
}