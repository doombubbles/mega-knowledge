﻿using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Primary;

public class DoubleRanga : MegaKnowledge
{
    public override string TowerId => TowerType.BoomerangMonkey;

    public override string Description => MegaKnowledgeMod.OpKnowledge
        ? "Boomerang Monkeys throw 2 Boomerangs at a time!"
        : "Boomerang Monkeys tier 3 and below throw 2 Boomerangs at a time!";

    public override int Offset => -800;

    public override void Apply(TowerModel model)
    {
        if (model.tier > 3 && !MegaKnowledgeMod.OpKnowledge) return;
        
        var weaponModel = model.GetAttackModel().weapons[0];
        var random = new RandomArcEmissionModel("RandomArcEmissionModel_", 2, 0, 0, 30, 1, null);
        var eM = new ArcEmissionModel("ArcEmissionModel_", 2, 0, 30, null, false, false);
        weaponModel.emission = eM;
    }
}