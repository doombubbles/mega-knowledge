using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data.Knowledge;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using MegaKnowledge;
using MelonLoader;
using ModHelperData = MegaKnowledge.ModHelperData;

[assembly: MelonInfo(typeof(MegaKnowledgeMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MegaKnowledge
{
    public class MegaKnowledgeMod : BloonsTD6Mod
    {
        public static MelonPreferences_Category MegaKnowledgeCategory;

        public static readonly ModSettingBool OpOvertime = new(false)
        {
            description =
                "Re-enables the old OP behavior of the Engineering Mega Knowledge \"Overtime\" also affecting the Engineer sentries.",
            icon = VanillaSprites.SentryGunUpgradeIcon,
            displayName = "OP Overtime"
        };

        public static readonly ModSettingBool OpCrystalBall = new(false)
        {
            description =
                "Wizards with the Guided Magic upgrade can still shoot through walls while using the \"Crystal Ball\" Mega Knowledge.",
            icon = VanillaSprites.GuildedMagicUpgradeIcon,
            displayName = "OP Crystal Ball"
        };

#if DEBUG
        public static readonly ModSettingButton CreateMds = new(GenerateReadme.Generate);
#endif

        private static readonly Dictionary<string, TowerModel> BackupTowerModels = new();

        public override void OnMainMenu()
        {
            foreach (var towerModel in Game.instance.model.towers)
            {
                foreach (var megaKnowledge in ModContent.GetContent<MegaKnowledge>().Where(megaKnowledge =>
                             towerModel.baseId == megaKnowledge.TowerId && megaKnowledge.TargetChanging))
                {
                    if (!BackupTowerModels.ContainsKey(towerModel.name))
                    {
                        BackupTowerModels[towerModel.name] = towerModel.Duplicate();
                    }

                    if (!megaKnowledge.Enabled)
                    {
                        Reset(towerModel);
                    }
                }
            }

            MegaKnowledgeCategory.SaveToFile(false);
        }

        private static void Reset(TowerModel towerModel)
        {
            var model = BackupTowerModels[towerModel.name].Duplicate();
            towerModel.behaviors = model.behaviors;
            towerModel.towerSelectionMenuThemeId = model.towerSelectionMenuThemeId;
            towerModel.targetTypes = model.targetTypes;
        }

        public override void OnUpdate()
        {
            if (InGame.instance != null && InGame.instance.bridge != null)
            {
                foreach (var megaKnowledge in ModContent.GetContent<MegaKnowledge>().Where(mk => mk.Enabled))
                {
                    megaKnowledge.OnUpdate();
                }
            }
        }

        [HarmonyPatch(typeof(GameModel), nameof(GameModel.CreateModded),
            typeof(Il2CppSystem.Collections.Generic.List<string>), typeof(ActiveRelicKnowledge), typeof(MapModel))]
        internal class GameModel_CreateModded
        {
            [HarmonyPrefix]
            internal static void Prefix(Il2CppSystem.Collections.Generic.List<string> activeMods)
            {
                var knowledge = !Game.instance.GetPlayerProfile().knowledgeDisabled;
                foreach (var activeMod in activeMods)
                {
                    knowledge &= Game.instance.model.mods.Where(modelMod => modelMod.name == activeMod)
                        .All(modelMod =>
                            modelMod.mutatorMods.All(mod => mod.TryCast<DisableMonkeyKnowledgeModModel>() == null));
                }

                foreach (var towerModel in Game.instance.model.towers)
                {
                    foreach (var megaKnowledge in ModContent.GetContent<MegaKnowledge>()
                                 .Where(megaKnowledge =>
                                     megaKnowledge.Enabled &&
                                     megaKnowledge.TargetChanging &&
                                     towerModel.baseId == megaKnowledge.TowerId))
                    {
                        if (!knowledge)
                        {
                            Reset(towerModel);
                        }
                        else
                        {
                            megaKnowledge.Apply(towerModel);
                        }
                    }
                }
            }

            [HarmonyPostfix]
            internal static void Postfix(ref GameModel __result,
                Il2CppSystem.Collections.Generic.List<string> activeMods)
            {
                var knowledge = !Game.instance.GetPlayerProfile().knowledgeDisabled;
                foreach (var activeMod in activeMods)
                {
                    knowledge &= Game.instance.model.mods.Where(modelMod => modelMod.name == activeMod)
                        .All(modelMod =>
                            modelMod.mutatorMods.All(mod => mod.TryCast<DisableMonkeyKnowledgeModModel>() == null));
                }

                if (!knowledge)
                {
                    return;
                }

                foreach (var towerModel in __result.towers)
                {
                    foreach (var megaKnowledge in ModContent.GetContent<MegaKnowledge>()
                                 .Where(megaKnowledge =>
                                     megaKnowledge.Enabled &&
                                     !megaKnowledge.TargetChanging &&
                                     towerModel.baseId == megaKnowledge.TowerId))
                    {
                        megaKnowledge.Apply(towerModel);
                    }
                }
            }
        }
    }
}