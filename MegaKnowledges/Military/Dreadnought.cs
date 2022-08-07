using System.Linq;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Military
{
    public class Dreadnought : MegaKnowledge
    {
        public override string TowerId => TowerType.MonkeyBuccaneer;
        public override string Description => "Monkey Buccaneers shoot molten cannon balls instead of darts and grapes.";
        public override int Offset => -400;

        public override void Apply(TowerModel model)
        {
            var attackModel = model.GetAttackModel();
            var flameGrape = Game.instance.model.GetTower(TowerType.MonkeyBuccaneer, 0, 2, 0).GetWeapons()[3]
                .projectile;
            foreach (var projectileModel in attackModel.GetDescendants<ProjectileModel>().ToList()
                         .Where(projectileModel => !projectileModel.name.Contains("Explosion") &&
                                                   projectileModel.GetDamageModel() != null))
            {
                projectileModel.collisionPasses = flameGrape.collisionPasses;
                projectileModel.AddBehavior(flameGrape.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
                if (model.appliedUpgrades.Contains("Buccaneer-Hot Shot"))
                {
                    projectileModel.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>()
                        .triggerImmediate = true;
                    projectileModel.scale = .5f;
                }
                else
                {
                    projectileModel.scale = .75f;
                }

                projectileModel.display = CreatePrefabReference("c840e245a0b1deb4284cfc3f953e16cf");
                projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }
        }
    }
}