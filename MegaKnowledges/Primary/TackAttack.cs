using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Primary;

public class TackAttack : MegaKnowledge
{
    public override string TowerId => TowerType.TackShooter;
    public override string Description => "Tack Shooter attacks constantly, and its projectiles travel farther.";
    public override int Offset => 0;

    public override void Apply(TowerModel model)
    {
        model.GetAttackModel().fireWithoutTarget = true;

        foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
        {
            var tsm = projectileModel.GetBehavior<TravelStraitModel>();
            if (tsm != null)
            {
                tsm.Lifespan *= 1.5f;
            }
        }

        if (model.appliedUpgrades.Contains(UpgradeType.RingOfFire))
        {
            model.range *= 1.5f;
        }
    }
}