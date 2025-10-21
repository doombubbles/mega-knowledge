using System;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Magic;

public class CrystalBall : MegaKnowledge
{
    public override string TowerId => TowerType.WizardMonkey;

    public override string Description =>
        MegaKnowledgeMod.OpKnowledge
            ? "The Guided Magic upgrade gives Wizard Monkeys Advanced Intel style targeting"
            : "Instead of letting Wizard Monkeys see through walls, the Guided Magic upgrade gives them Advanced Intel style targeting.";

    public override int Offset => -2000;

    public override void Apply(TowerModel model)
    {
        if (!model.appliedUpgrades.Contains(UpgradeType.GuidedMagic)) return;

        if (!MegaKnowledgeMod.OpKnowledge)
        {
            model.ignoreBlockers = false;
        }

        var guidedMagic = model.GetWeapon().projectile.GetBehavior<TrackTargetModel>();
        foreach (var attackModel in model.GetAttackModels())
        {
            if (attackModel.GetBehavior<TargetFirstPrioCamoModel>() != null)
            {
                attackModel.RemoveBehavior<TargetFirstPrioCamoModel>();
                attackModel.AddBehavior(new TargetFirstSharedRangeModel("",
                    true, true, false, false, false));
            }

            if (attackModel.GetBehavior<TargetLastPrioCamoModel>() != null)
            {
                attackModel.RemoveBehavior<TargetLastPrioCamoModel>();
                attackModel.AddBehavior(new TargetLastSharedRangeModel("",
                    true, true, false, false, false));
            }

            if (attackModel.GetBehavior<TargetClosePrioCamoModel>() != null)
            {
                attackModel.RemoveBehavior<TargetClosePrioCamoModel>();
                attackModel.AddBehavior(new TargetCloseSharedRangeModel("",
                    true, true, false, false, false));
            }

            if (attackModel.GetBehavior<TargetStrongPrioCamoModel>() != null)
            {
                attackModel.RemoveBehavior<TargetStrongPrioCamoModel>();
                attackModel.AddBehavior(new TargetStrongSharedRangeModel("",
                    true, true, false, false, false));
            }

            if (!MegaKnowledgeMod.OpKnowledge)
            {
                attackModel.attackThroughWalls = false;
            }
        }

        foreach (var weaponModel in model.GetWeapons())
        {
            weaponModel.emission.AddBehavior(
                new EmissionCamoIfTargetIsCamoModel("EmissionCamoIfTargetIsCamoModel_CamoEmissionBehavior"));
        }

        foreach (var projectileModel in model.GetDescendants<ProjectileModel>().ToList())
        {
            var travelStraitModel = projectileModel.GetBehavior<TravelStraitModel>();
            if (travelStraitModel != null)
            {
                var newLifeSpan = travelStraitModel.Lifespan * (200 / travelStraitModel.Speed);
                travelStraitModel.Lifespan = Math.Max(travelStraitModel.Lifespan, newLifeSpan);
                if (projectileModel.GetBehavior<TrackTargetModel>() == null)
                {
                    projectileModel.AddBehavior(guidedMagic.Duplicate());
                }
            }

            if (!MegaKnowledgeMod.OpKnowledge)
            {
                projectileModel.ignoreBlockers = false;
            }
        }
    }
}