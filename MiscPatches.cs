using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons.Behaviors;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Behaviors;
using Il2CppAssets.Scripts.Simulation.Display;
using Il2CppAssets.Scripts.Simulation.Input;
using Il2CppAssets.Scripts.Simulation.SMath;
using MegaKnowledge.MegaKnowledges.Military;
using MegaKnowledge.MegaKnowledges.Support;

namespace MegaKnowledge;

public static class MiscPatches
{

    [HarmonyPatch(typeof(RotateToPointer), nameof(RotateToPointer.SetRotation))]
    internal static class RotateToPointer_SetRotation
    {
        [HarmonyPrefix]
        internal static bool Prefix(RotateToPointer __instance)
        {
            var attack = __instance.attack;
            if (!ModContent.GetInstance<DartlingEmpowerment>().Enabled ||
                !attack.activeTargetSupplier.Is(out var target) ||
                !attack.HasAttackBehavior<RotateToPointer>() ||
                !attack.HasAttackBehavior<RotateToTarget>()) return true;

            var targetsBloon = target.Is<TargetFirst>() ||
                               target.Is<TargetLast>() ||
                               target.Is<TargetClose>() ||
                               target.Is<TargetStrong>() ||
                               target.Is<TargetCamo>();

            return !targetsBloon;
        }
    }

    [HarmonyPatch(typeof(RotateToTarget), nameof(RotateToTarget.ApplyRotation))]
    internal static class RotateToTarget_ApplyRotation
    {
        [HarmonyPrefix]
        internal static bool Prefix(RotateToTarget __instance)
        {
            var attack = __instance.attack;
            if (!ModContent.GetInstance<DartlingEmpowerment>().Enabled ||
                !attack.activeTargetSupplier.Is(out var target) ||
                !attack.HasAttackBehavior<RotateToPointer>() ||
                !attack.HasAttackBehavior<RotateToTarget>()) return true;

            var targetsBloon = target.Is<TargetFirst>() ||
                               target.Is<TargetLast>() ||
                               target.Is<TargetClose>() ||
                               target.Is<TargetStrong>() ||
                               target.Is<TargetCamo>();

            return targetsBloon;
        }
    }

    [HarmonyPatch(typeof(TrackTargetWithinTime), nameof(TrackTargetWithinTime.CalculateDirection))]
    internal class TrackTargetWithinTime_CalculateDirection
    {
        [HarmonyPrefix]
        internal static bool Prefix(TrackTargetWithinTime __instance)
        {
            if (__instance.trackTargetWithinTimeModel.name.Contains("Behind") &&
                __instance.projectile.ExhaustFraction < .2)
            {
                return false;
            }

            return true;
        }
    }
}