using System;
using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Data.Knowledge;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Gameplay.Mods;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MegaKnowledge;
using MelonLoader;
[assembly: MelonInfo(typeof(MegaKnowledgeMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MegaKnowledge;

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
    
    public static readonly ModSettingBool OpSpikeEmpowerment = new(false)
    {
        description =
            "Re-enables the old behavior of the \"Spike Empowerment\" Mega Knowledge letting Spike Factories target anywhere on the map.",
        icon = VanillaSprites.SpikeFactoryIcon,
        displayName = "OP Spike Empowerment"
    };

#if DEBUG
    public static readonly ModSettingButton CreateMds = new(GenerateReadme.Generate);
#endif

    public override void OnMainMenu()
    {
        MegaKnowledgeCategory.SaveToFile(false);
    }

    public override void OnUpdate()
    {
        if (InGame.instance == null || InGame.instance.bridge == null) return;

        foreach (var megaKnowledge in ModContent.GetContent<MegaKnowledge>().Where(mk => mk.Enabled))
        {
            megaKnowledge.OnUpdate();
        }
    }

    public override void OnNewGameModel(GameModel result, Il2CppSystem.Collections.Generic.List<ModModel> mods)
    {
        var knowledgeDisabled = Game.instance.GetPlayerProfile().knowledgeDisabled ||
                                mods.ToList().Any(model =>
                                    model.mutatorMods.Any(mod => mod.Is<DisableMonkeyKnowledgeModModel>()));

        if (knowledgeDisabled) return;

        foreach (var towerModel in result.towers)
        {
            var megaKnowledges = ModContent.GetContent<MegaKnowledge>()
                .Where(megaKnowledge => megaKnowledge.Enabled && towerModel.baseId == megaKnowledge.TowerId);

            foreach (var megaKnowledge in megaKnowledges)
            {
                try
                {
                    megaKnowledge.Apply(towerModel);
                }
                catch (Exception e)
                {
                    ModHelper.Warning<MegaKnowledgeMod>($"Failed to apply {megaKnowledge.Id}");
                    ModHelper.Warning<MegaKnowledgeMod>(e);
                }
            }
        }
    }
}