using System.Linq;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;

namespace MegaKnowledge.MegaKnowledges.Military;

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
            if (!projectileModel.HasBehavior<AddBehaviorToBloonModel>())
            {
                projectileModel.AddBehavior(flameGrape.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
            }
            if (model.appliedUpgrades.Contains("Buccaneer-Hot Shot"))
            {
                projectileModel.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>()
                    .triggerImmediate = true;
            }

            projectileModel.scale = projectileModel.radius > 3 ? .7f : .5f;
            projectileModel.display = CreatePrefabReference("c840e245a0b1deb4284cfc3f953e16cf");
            projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }
    }
}