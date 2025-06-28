using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

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

        var attackModel = model.GetAttackModel();

        MegaKnowledgeMod.UpdatePointer(attackModel);
        MegaKnowledgeMod.AddAllTargets(attackModel);

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