using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Filters;

namespace MegaKnowledge.MegaKnowledges.Primary;

public class BombVoyage : MegaKnowledge
{
    public override string TowerId => TowerType.BombShooter;

    public override string Description =>
        MegaKnowledgeMod.OpKnowledge
            ? "Bomb Shooters' projectiles travel much faster and can damage any Bloon type."
            : "Bomb Shooters' projectiles travel much faster, and explosions can splash damage onto Camo bloons.";

    public override int Offset => -400;

    public override void Apply(TowerModel model)
    {
        var projectile = model.GetAttackModel().weapons[0].projectile;
        var travelStraitModel = projectile.GetBehavior<TravelStraitModel>();
        var createProjectileOnContactModel = projectile.GetBehavior<CreateProjectileOnContactModel>();
        if (travelStraitModel != null && createProjectileOnContactModel != null)
        {
            travelStraitModel.Speed *= MegaKnowledgeMod.OpKnowledge ? 2 : 1.5f;
        }

        if (MegaKnowledgeMod.OpKnowledge)
        {
            model.GetDescendants<DamageModel>().ForEach(damageModel =>
            {
                damageModel.immuneBloonProperties = BloonProperties.None;
            });
        }
        else
        {
            model.GetDescendants<ProjectileModel>().ForEach(p =>
                p.GetDescendants<FilterInvisibleModel>().ForEach(invisibleModel => invisibleModel.isActive = false)
            );
        }
    }
}