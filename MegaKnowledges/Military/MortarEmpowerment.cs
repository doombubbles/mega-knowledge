using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Military
{
    public class MortarEmpowerment : MegaKnowledge
    {
        public override string TowerId => TowerType.MortarMonkey;
        public override string Description => "Mortar Monkey can attack like a regular tower.";
        public override int Offset => 800;
        public override bool TargetChanging => true;

        public override void Apply(TowerModel model)
        {
            var boomer = Game.instance.model.GetTowerFromId(TowerType.BoomerangMonkey);
            var attackModel = model.GetAttackModel();
            foreach (var boomerTargetType in boomer.targetTypes)
            {
                model.targetTypes = model.targetTypes.AddTo(boomerTargetType);
            }


            var targetSelectedPointModel = attackModel.GetBehavior<TargetSelectedPointModel>();
            attackModel.RemoveBehavior<TargetSelectedPointModel>();
            attackModel.targetProvider = null;

            attackModel.AddBehavior(boomer.GetAttackModel().GetBehavior<TargetFirstModel>().Duplicate());
            attackModel.AddBehavior(boomer.GetAttackModel().GetBehavior<TargetLastModel>().Duplicate());
            attackModel.AddBehavior(boomer.GetAttackModel().GetBehavior<TargetCloseModel>().Duplicate());
            attackModel.AddBehavior(boomer.GetAttackModel().GetBehavior<TargetStrongModel>().Duplicate());

            attackModel.AddBehavior(targetSelectedPointModel);

            model.towerSelectionMenuThemeId = "ActionButton";
        }
    }
}