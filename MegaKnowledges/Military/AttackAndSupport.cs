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
        if (!model.HasBehavior(out SubmergeModel submerge) || model.isParagon) return;

        model.targetTypes = Game.instance.model.GetTowerFromId(TowerType.MonkeySub).targetTypes;

        var submergeEffect = model.GetBehavior<SubmergeEffectModel>().effectModel;

        if (submerge.heroXpScale > 1.0)
        {
            model.AddBehavior(HeroXpScaleSupportModel.Create(new()
            {
                name = "HeroXpScaleSupportModel_",
                isUnique = true,
                heroXpScale = submerge.heroXpScale,
                buffLocsName = submerge.buffLocsName,
                buffIconName = submerge.buffIconName
            }));
        }

        if (submerge.abilityCooldownSpeedScale > 1.0)
        {
            model.AddBehavior(AbilityCooldownScaleSupportModel.Create(new()
            {
                name = "AbilityCooldownScaleSupportModel_",
                isUnique = true,
                abilityCooldownSpeedScale = submerge.abilityCooldownSpeedScale,
                affectsOnlyWater = true,
                buffLocsName = submerge.buffLocsName,
                buffIconName = submerge.buffIconName,
                mutatorPriority = submerge.supportMutatorPriority
            }));
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

        model.AddBehavior(CreateEffectAfterTimeModel.Create(new()
        {
            name = "CreateEffectAfterTimeModel_",
            effectModel = submergeEffect,
            useRoundTime = true
        }));
    }
}