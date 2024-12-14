using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Support;

public class SpikeEmpowerment : MegaKnowledge
{
    public override string TowerId => TowerType.SpikeFactory;

    public override string Description => "Spike Factories choose the spot where their spikes land.";

    public override int Offset => 400;

    public override void Apply(TowerModel model)
    {
        model.towerSelectionMenuThemeId = TowerType.MortarMonkey;

        model.GetAttackModel().RemoveBehavior<TargetTrackModel>();
        model.GetAttackModel().RemoveBehavior<SmartTargetTrackModel>();
        model.GetAttackModel().RemoveBehavior<CloseTargetTrackModel>();
        model.GetAttackModel().RemoveBehavior<FarTargetTrackModel>();


        var targetSelectedPointModel = model.GetAttackModel().GetBehavior<TargetSelectedPointModel>();
        if (targetSelectedPointModel == null)
        {
            var tspm = new TargetSelectedPointModel("TargetSelectedPointModel_", true,
                false, CreatePrefabReference("f786dd2ad0e3e8649a8ff0ac9f8cc6fb"), 1, "",
                !MegaKnowledgeMod.OpKnowledge, !MegaKnowledgeMod.OpKnowledge, CreatePrefabReference("d053160180f53da43be4f9972ee1497a"),
                true, null, true);
            model.GetAttackModel().AddBehavior(tspm);
        }

        model.UpdateTargetProviders();
    }
}