using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Military;

public class MortarEmpowerment : MegaKnowledge
{
    public override string TowerId => TowerType.MortarMonkey;
    public override string Description => "Mortar Monkey can attack like a regular tower.";
    public override int Offset => 800;

    public override void Apply(TowerModel model)
    {
        var normalAttack = Game.instance.model.GetTowerFromId(TowerType.BoomerangMonkey).GetAttackModel();
        var attackModel = model.GetAttackModel();

        var targetSelectedPointModel = attackModel.GetBehavior<TargetSelectedPointModel>();
        attackModel.RemoveBehavior<TargetSelectedPointModel>();
        attackModel.targetProvider = null;

        attackModel.AddBehavior(normalAttack.GetBehavior<TargetFirstModel>().Duplicate());
        attackModel.AddBehavior(normalAttack.GetBehavior<TargetLastModel>().Duplicate());
        attackModel.AddBehavior(normalAttack.GetBehavior<TargetCloseModel>().Duplicate());
        attackModel.AddBehavior(normalAttack.GetBehavior<TargetStrongModel>().Duplicate());

        attackModel.AddBehavior(targetSelectedPointModel);
        
        model.UpdateTargetProviders();

        model.towerSelectionMenuThemeId = "ActionButton";
    }
}