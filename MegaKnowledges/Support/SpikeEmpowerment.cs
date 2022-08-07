using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;
using UnhollowerBaseLib;

namespace MegaKnowledge.MegaKnowledges.Support
{
    public class SpikeEmpowerment : MegaKnowledge
    {
        public override string TowerId => TowerType.SpikeFactory;

        public override string Description =>
            "Spike Factories choose the spot where their spikes land, and spikes damage Bloons while traveling.";

        public override int Offset => 400;
        public override bool TargetChanging => true;

        public override void Apply(TowerModel model)
        {
            var mortar = Game.instance.model.GetTowerFromId(TowerType.MortarMonkey);
            model.towerSelectionMenuThemeId = TowerType.MortarMonkey;

            model.targetTypes = new Il2CppReferenceArray<TargetType>(mortar.targetTypes);

            if (model.targetTypes.Length > 1)
            {
                while (model.targetTypes.Length > 1)
                {
                    model.targetTypes = model.targetTypes.RemoveItemOfType<TargetType, TargetType>();
                }

                model.targetTypes[0].id = "TargetSelectedPoint";
                model.targetTypes[0].intID = -1;
                model.targetTypes[0].actionOnCreate = true;
                model.targetTypes[0].isActionable = true;
            }


            model.GetAttackModel().RemoveBehavior<TargetTrackModel>();
            model.GetAttackModel().RemoveBehavior<SmartTargetTrackModel>();
            model.GetAttackModel().RemoveBehavior<CloseTargetTrackModel>();
            model.GetAttackModel().RemoveBehavior<FarTargetTrackModel>();


            var targetSelectedPointModel = model.GetAttackModel().GetBehavior<TargetSelectedPointModel>();
            if (targetSelectedPointModel == null)
            {
                var tspm = new TargetSelectedPointModel("TargetSelectedPointModel_", true,
                    false, CreatePrefabReference("4e88dd78c6e800d41a6df5b02d592082"), .5f, "",
                    false, false, CreatePrefabReference(""), true, null);
                model.GetAttackModel().AddBehavior(tspm);
            }

            model.UpdateTargetProviders();
            model.GetDescendant<ArriveAtTargetModel>().filterCollisionWhileMoving = false;
        }
    }
}