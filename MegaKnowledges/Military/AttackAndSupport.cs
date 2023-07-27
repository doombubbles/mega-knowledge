using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Military;

public class AttackAndSupport : MegaKnowledge
{
    public override string TowerId => TowerType.MonkeySub;
    public override string Description => "Monkey Subs don't need to submerge to gain submerged benefits.";
    public override int Offset => -800;

    public override void Apply(TowerModel model)
    {
        if (model.GetBehavior<SubmergeModel>() == null) return;

        model.targetTypes = Game.instance.model.GetTowerFromId(TowerType.MonkeySub).targetTypes;

        var submergeEffect = model.GetBehavior<SubmergeEffectModel>().effectModel;
        var submerge = model.GetBehavior<SubmergeModel>();

        if (submerge.heroXpScale > 1.0)
        {
            model.AddBehavior(new HeroXpScaleSupportModel("HeroXpScaleSupportModel_", true, submerge.heroXpScale,
                null, submerge.buffLocsName, submerge.buffIconName));
        }

        if (submerge.abilityCooldownSpeedScale > 1.0)
        {
            model.AddBehavior(new AbilityCooldownScaleSupportModel("AbilityCooldownScaleSupportModel_",
                true, submerge.abilityCooldownSpeedScale, true, false, null,
                submerge.buffLocsName, submerge.buffIconName, false, submerge.supportMutatorPriority));
        }

        model.RemoveBehavior<SubmergeModel>();

        foreach (var attackModel in model.GetAttackModels())
        {
            if (attackModel.name.Contains("Submerge"))
            {
                attackModel.name = attackModel.name.Replace("Submerged", "");
                attackModel.weapons[0].GetBehavior<EjectEffectModel>().effectModel.assetId =
                    submerge.attackDisplayPath;
            }

            attackModel.RemoveBehavior<SubmergedTargetModel>();
        }

        model.AddBehavior(new CreateEffectAfterTimeModel("CreateEffectAfterTimeModel_", submergeEffect, 0f, true));
    }
}