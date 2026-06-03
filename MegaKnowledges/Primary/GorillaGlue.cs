using System;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;

namespace MegaKnowledge.MegaKnowledges.Primary;

public class GorillaGlue : MegaKnowledge
{
    public override string TowerId => TowerType.GlueGunner;
    public override string Description => "Glue Gunners' glue globs do moderate damage themselves.";
    public override int Offset => 800;

    public override void Apply(TowerModel model)
    {
        foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
        {
            var amount = Math.Max(1, model.tier);
            if (model.tier == 4)
            {
                amount++;
            }

            if (model.tier == 5)
            {
                amount += 5;
            }

            var damageModel = projectileModel.GetDamageModel();
            if (damageModel == null)
            {
                damageModel = DamageModel.Create(new() { damage = amount });
                projectileModel.AddBehavior(damageModel);
            }
            else
            {
                damageModel.damage += amount;
            }

            if (model.appliedUpgrades.Contains(UpgradeType.MOABGlue))
            {
                var damageModifierForTagModel = DamageModifierForTagModel.Create(new()
                {
                    tag = BloonTag.Moabs,
                    damageAddative = amount * 9,
                    applyOverMaxDamage = true
                });
                projectileModel.AddBehavior(damageModifierForTagModel);

                projectileModel.hasDamageModifiers = true;
            }
        }
    }
}