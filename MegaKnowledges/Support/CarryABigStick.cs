using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;

namespace MegaKnowledge.MegaKnowledges.Support;

public class CarryABigStick : MegaKnowledge
{
    public override string Description => "Beast Handlers keep their 000 melee attack, and it gets stronger as you upgrade them.";
    public override string TowerId => TowerType.BeastHandler;
    public override int Offset => 1600;

    public override void Apply(TowerModel model)
    {
        if (model.tier == 0) return;

        var baseAttack = Game.instance.model.GetTower(TowerType.BeastHandler).GetAttackModel();
        var weapon = baseAttack.weapons[0].Duplicate();
        var proj = weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;

        var attackModel = model.GetAttackModel();
        attackModel.AddWeapon(weapon);
        attackModel.range = baseAttack.range;

        // Fish upgrades give attack speed and splash size
        weapon.Rate /= 1 + model.tiers[0] * model.tiers[0] * .25f;
        proj.radius += model.tier * 4;

        // Dino upgrades give damage
        proj.GetDamageModel().damage += model.tiers[1] * model.tiers[1];
        
        // Bird upgrades give range and pierce
        attackModel.range += model.tiers[2] * 5;
        proj.pierce += model.tiers[2] * model.tiers[2];
    }
}