using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Primary
{
    public class SplodeyDarts : MegaKnowledge
    {
        public override string TowerId => TowerType.DartMonkey;
        public override string Description => "Dart Monkey projectiles explode on expiration.";
        public override int Offset => -1200;

        public override void Apply(TowerModel model)
        {
            foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
            {
                var bomb = Game.instance.model.GetTowerFromId(TowerType.BombShooter).GetWeapon()
                    .projectile.Duplicate();
                var pb = bomb.GetBehavior<CreateProjectileOnContactModel>();
                var sound = bomb.GetBehavior<CreateSoundOnProjectileCollisionModel>();
                var effect = bomb.GetBehavior<CreateEffectOnContactModel>();


                if (model.appliedUpgrades.Contains(UpgradeType.EnhancedEyesight))
                {
                    pb.projectile.GetBehavior<ProjectileFilterModel>().filters
                        .GetItemOfType<FilterModel, FilterInvisibleModel>().isActive = false;
                }

                /*pb.name = "CreateProjectileOnContactModel_SplodeyDarts";
                sound.name = "CreateSoundOnProjectileCollisionModel_SplodeyDarts";
                effect.name = "CreateEffectOnContactModel_SplodeyDarts";
                projectileModel.AddBehavior(pb);
                projectileModel.AddBehavior(sound);
                projectileModel.AddBehavior(effect);*/

                var behavior = new CreateProjectileOnExhaustFractionModel(
                    "CreateProjectileOnExhaustFractionModel_SplodeyDarts",
                    pb.projectile, pb.emission, 1f, 1f, true);
                projectileModel.AddBehavior(behavior);

                var soundBehavior = new CreateSoundOnProjectileExhaustModel(
                    "CreateSoundOnProjectileExhaustModel_SplodeyDarts",
                    sound.sound1, sound.sound2, sound.sound3, sound.sound4, sound.sound5);
                projectileModel.AddBehavior(soundBehavior);

                var eB = new CreateEffectOnExhaustedModel("CreateEffectOnExhaustedModel_SplodeyDarts", "", 0f, false,
                    false, effect.effectModel);
                projectileModel.AddBehavior(eB);
            }
        }
    }
}