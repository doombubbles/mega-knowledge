using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Primary
{
    public class BombVoyage : MegaKnowledge
    {
        public override string TowerId => TowerType.BombShooter;
        public override string Description => "Bomb Shooters' projectiles travel much faster and can damage any Bloon type.";
        public override int Offset => -400;

        public override void Apply(TowerModel model)
        {
            foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
            {
                var damageModel = projectileModel.GetBehavior<DamageModel>();
                if (damageModel != null)
                {
                    damageModel.immuneBloonProperties = BloonProperties.None;
                }

                var travelStraitModel = projectileModel.GetBehavior<TravelStraitModel>();
                var createProjectileOnContactModel = projectileModel.GetBehavior<CreateProjectileOnContactModel>();
                if (travelStraitModel != null && createProjectileOnContactModel != null)
                {
                    travelStraitModel.Speed *= 2;
                }
            }
        }
    }
}