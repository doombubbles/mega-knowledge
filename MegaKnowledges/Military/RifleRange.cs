﻿using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.GenericBehaviors;

namespace MegaKnowledge.MegaKnowledges.Military;

public class RifleRange : MegaKnowledge
{
    public override string TowerId => TowerType.SniperMonkey;
    public override string Description => "Sniper Monkey shots can critically strike for double damage.";
    public override int Offset => -1200;

    public override void Apply(TowerModel model)
    {
        var damage = model.GetWeapon().projectile.GetDamageModel().damage;
        model.GetWeapon().AddBehavior(new CritMultiplierModel("", damage * 2, 1, 6,
            new DisplayModel("Crit", CreatePrefabReference("252e82e70578330429a758339e10fd25"), 0, DisplayCategory.Effect),
            true));

        model.GetWeapon().projectile.AddBehavior(new ShowTextOnHitModel("",
            CreatePrefabReference("3dcdbc19136c60846ab944ada06695c0"), 0.5f, false, ""));
    }
}