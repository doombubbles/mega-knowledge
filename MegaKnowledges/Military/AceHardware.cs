using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Military;

public class AceHardware : MegaKnowledge
{
    public override string TowerId => TowerType.MonkeyAce;
    public override string Description => "Monkey Aces get a new shorter ranged focus-firing gunner attack.";
    public override int Offset => 0;

    public override void Apply(TowerModel model)
    {
        var towerModel = Game.instance.model.GetTower(TowerType.MonkeyAce, 0, 0, 4);
        var attack = towerModel.GetAttackModel("Spectre").Duplicate(Name);
        var weapon = attack.weapons[0]!;
        weapon.RemoveBehavior<AlternateProjectileModel>();
        attack.range = 60 + 20 * model.tier;
        weapon.Rate = .6f - .1f * model.tier;
        weapon.projectile.GetDamageModel().damage = 1 + model.tier / 2;
        weapon.projectile.pierce = model.GetAttackModels().First(a => !a.name.Contains("Spectre")).weapons[0]!.projectile.pierce;
        if (model.appliedUpgrades.Contains(UpgradeType.SpyPlane))
        {
            attack.GetDescendants<FilterInvisibleModel>().ForEach(m => m.isActive = false);
        }

        model.AddBehavior(attack);
    }
}