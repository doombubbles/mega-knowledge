using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Magic
{
    public class BloonAreNotPrepared : MegaKnowledge
    {
        public override string TowerId => TowerType.Druid;
        public override string Description => "Druids' personal stacking buffs always have maximum effect.";
        public override int Offset => -400;

        public override void Apply(TowerModel model)
        {
            if (model.appliedUpgrades.Contains(UpgradeType.HeartOfVengeance))
            {
                foreach (var weaponModel in model.GetWeapons())
                {
                    var lbasm = weaponModel.GetBehavior<LifeBasedAttackSpeedModel>();
                    if (lbasm != null)
                    {
                        var bonus = lbasm.lifeCap * lbasm.ratePerLife + lbasm.baseRateIncrease;
                        weaponModel.Rate /= 1 + bonus;
                        weaponModel.RemoveBehavior<LifeBasedAttackSpeedModel>();
                    }
                }
            }

            if (model.appliedUpgrades.Contains(UpgradeType.DruidOfWrath))
            {
                var dbasm = model.GetBehavior<DamageBasedAttackSpeedModel>();
                if (dbasm != null)
                {
                    var bonus = dbasm.maxStacks * dbasm.increasePerThreshold;
                    foreach (var weaponModel in model.GetWeapons())
                    {
                        weaponModel.Rate /= 1 + bonus;
                    }
                }
            }

            if (model.appliedUpgrades.Contains(UpgradeType.AvatarOfWrath))
            {
                var dvem = model.GetBehavior<DruidVengeanceEffectModel>();
                if (dvem != null)
                {
                    var dmwm = dvem.damageModifierWrathModel;
                    dmwm.rbeThreshold = 1;
                    dvem.epicGlowEffectStacks = -1;
                }
            }
        }
    }
}