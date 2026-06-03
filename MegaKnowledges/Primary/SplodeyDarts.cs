using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;

namespace MegaKnowledge.MegaKnowledges.Primary;

public class SplodeyDarts : MegaKnowledge
{
    public override string TowerId => TowerType.DartMonkey;

    public override string Description => MegaKnowledgeMod.OpKnowledge
        ? "Dart Monkey projectiles explode on expiration."
        : "Dart Monkey projectiles for Tier 3 and below explode on expiration.";

    public override int Offset => -1200;

    public override void Apply(TowerModel model)
    {
        if (model.tier > 3 && !MegaKnowledgeMod.OpKnowledge) return;

        var baseBomb = Game.instance.model.GetTowerFromId(TowerType.BombShooter).GetWeapon().projectile;
        foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
        {
            var bomb = baseBomb.Duplicate();
            var pb = bomb.GetBehavior<CreateProjectileOnContactModel>();
            var sound = bomb.GetBehavior<CreateSoundOnProjectileCollisionModel>();
            var effect = bomb.GetBehavior<CreateEffectOnContactModel>();


            if (model.appliedUpgrades.Contains(UpgradeType.EnhancedEyesight))
            {
                pb.projectile.GetDescendant<FilterInvisibleModel>().isActive = false;
            }

            var behavior = CreateProjectileOnExhaustFractionModel.Create(new()
            {
                name = "SplodeyDarts",
                projectile = pb.projectile,
                emission = pb.emission,
                fraction = 1f,
                durationfraction = 1f,
                canCreateInBetweenRounds = true
            });
            projectileModel.AddBehavior(behavior);

            var soundBehavior = CreateSoundOnProjectileExhaustModel.Create(new()
            {
                name = "SplodeyDarts",
                sound1 = sound.sound1,
                sound2 = sound.sound2,
                sound3 = sound.sound3,
                sound4 = sound.sound4,
                sound5 = sound.sound5
            });
            projectileModel.AddBehavior(soundBehavior);

            var eB = CreateEffectOnExhaustedModel.Create(new()
            {
                name = "SplodeyDarts",
                effectModel = effect.effectModel
            });
            projectileModel.AddBehavior(eB);

            if (!MegaKnowledgeMod.OpKnowledge)
            {
                pb.projectile.pierce = 5;
            }
        }
    }
}