using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace MegaKnowledge.MegaKnowledges.Military;

public class DartlingEmpowerment : MegaKnowledge
{
    public override string TowerId => TowerType.DartlingGunner;
    public override string Description => "Dartling Gunner can attack like a regular tower.";
    public override int Offset => 1200;

    public override void Apply(TowerModel model)
    {
        if (model.appliedUpgrades.Contains(UpgradeType.BloonAreaDenialSystem))
        {
            return;
        }

        var normalAttack = Game.instance.model.GetTowerFromId(TowerType.BoomerangMonkey).GetAttackModel();
        var attackModel = model.GetAttackModel();

        attackModel.AddBehavior(normalAttack.GetBehavior<RotateToTargetModel>().Duplicate());

        var targetPointerModel = attackModel.GetBehavior<TargetPointerModel>();
        var targetSelectedPointModel = attackModel.GetBehavior<TargetSelectedPointModel>();

        attackModel.RemoveBehavior<TargetPointerModel>();
        attackModel.RemoveBehavior<TargetSelectedPointModel>();

        attackModel.AddBehavior(normalAttack.GetBehavior<TargetFirstModel>().Duplicate());
        attackModel.AddBehavior(normalAttack.GetBehavior<TargetLastModel>().Duplicate());
        attackModel.AddBehavior(normalAttack.GetBehavior<TargetCloseModel>().Duplicate());
        attackModel.AddBehavior(normalAttack.GetBehavior<TargetStrongModel>().Duplicate());

        attackModel.AddBehavior(targetPointerModel);
        attackModel.AddBehavior(targetSelectedPointModel);

        model.UpdateTargetProviders();

        if (model.appliedUpgrades.Contains(UpgradeType.FasterSwivel))
        {
            var travelStraitModel = attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>();
            if (travelStraitModel != null)
            {
                travelStraitModel.Speed *= 47f / 35f;
            }
        }
    }
}