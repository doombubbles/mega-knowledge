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
using MegaKnowledge.MegaKnowledges.Support;

namespace MegaKnowledge;

public static class MiscPatches
{
    [HarmonyPatch(typeof(RotateToPointer), nameof(RotateToPointer.SetRotation))]
    internal class RotateToPointer_SetRotation
    {
        [HarmonyPrefix]
        internal static bool Prefix(RotateToPointer __instance)
        {
            if (__instance.dartlingMaintainLastPos is { tower: not null })
            {
                return __instance.dartlingMaintainLastPos.tower.TargetType.intID == -1;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(LineEffect), nameof(LineEffect.UpdateEffect))]
    internal class LineEffect_UpdateEffect
    {
        [HarmonyPrefix]
        internal static bool Prefix(LineEffect __instance)
        {
            if (__instance.rotateToPointer?.dartlingMaintainLastPos?.tower != null)
            {
                if (__instance.rotateToPointer.dartlingMaintainLastPos.tower.TargetType.intID != -1 &&
                    __instance.rotateToPointer.attack.target.bloon != null &&
                    !__instance.lineEffectModel.isLineDisplayEndless)
                {
                    __instance.targetMagnitude =
                        __instance.rotateToPointer.attack.target.bloon.Position.Distance(__instance.rotateToPointer
                            .dartlingMaintainLastPos.tower.Position) -
                        20;
                }
            }

            return true;
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

    [HarmonyPatch(typeof(InputManager), nameof(InputManager.GetRangeMeshes))]
    internal static class InputManager_GetRangeMeshes
    {
        [HarmonyPostfix]
        private static void Postfix(InputManager __instance, TowerModel towerModel,
            Vector3 position, ref Il2CppSystem.Collections.Generic.List<Mesh> __result)
        {
            if (towerModel != null &&
                towerModel.baseId == TowerType.BeastHandler &&
                towerModel.tier > 0 &&
                ModContent.GetInstance<CarryABigStick>().Enabled)
            {
                
                var mesh = RangeMesh.GetMeshStatically(__instance.Sim, position, towerModel.GetAttackModel().range,
                    towerModel.ignoreBlockers);
                mesh.isValid = true;
                mesh.position = position;
                __result.Add(mesh);
            }
        }
    }
}