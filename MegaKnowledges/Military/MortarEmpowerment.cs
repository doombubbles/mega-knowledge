using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Military;

public class MortarEmpowerment : MegaKnowledge
{
    public override string TowerId => TowerType.MortarMonkey;
    public override string Description => "Mortar Monkey can attack like a regular tower.";
    public override int Offset => 800;

    public override void Apply(TowerModel model)
    {
        var attackModel = model.GetAttackModel();

        MegaKnowledgeMod.AddAllTargets(attackModel);

        model.towerSelectionMenuThemeId = "ActionButton";
        model.UpdateTargetProviders();
    }
}