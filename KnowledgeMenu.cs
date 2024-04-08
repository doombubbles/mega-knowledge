using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.Knowledge;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.TowerSets;
using UnityEngine;
using UnityEngine.UI;
using static Il2CppAssets.Scripts.Models.TowerSets.TowerSet;
using Object = UnityEngine.Object;

namespace MegaKnowledge;

public class KnowledgeMenu
{
    public static KnowledgeSkillTree knowledgeSkillTree;

    public static readonly Dictionary<TowerSet, Dictionary<string, KnowledgeSkillBtn>> Buttons = new();

    [HarmonyPatch(typeof(KnowledgeSkillTree), nameof(KnowledgeSkillTree.UpdateButtonStates))]
    internal class KnowledgeSkillTree_UpdateButtonStates
    {
        [HarmonyPostfix]
        internal static void Postfix(KnowledgeSkillTree __instance)
        {
            knowledgeSkillTree = __instance;

            foreach (var megaKnowledge in ModContent.GetContent<MegaKnowledge>())
            {
                var cloneFrom = __instance.GetComponentsInChildren<Component>()
                    .First(component => component.name == megaKnowledge.KnowledgeToCloneFrom).gameObject;

                var child = cloneFrom.transform.parent.FindChild(megaKnowledge.Name);
                if (child != null)
                {
                    return;
                }

                var newButton = Object.Instantiate(cloneFrom, cloneFrom.transform.parent, true);
                newButton.transform.Translate(megaKnowledge.Offset, -400, 0);

                var btn = newButton.GetComponentInChildren<KnowledgeSkillBtn>();
                newButton.name = megaKnowledge.Name;
                var knowledgeModels = Game.instance.model.allKnowledge
                    .Where(model => model.category.ToString() == megaKnowledge.TowerSet.ToString());
                btn.ClickedEvent = new Action<KnowledgeSkillBtn>(skillBtn =>
                {
                    var hasAll = true;
                    var btd6Player = Game.instance.GetBtd6Player();
                    foreach (var knowledgeModel in knowledgeModels)
                    {
                        if (!btd6Player.HasKnowledge(knowledgeModel.name))
                        {
                            hasAll = false;
                        }
                    }

                    if (!(Input.GetKey(KeyCode.LeftShift) || hasAll))
                    {
                        foreach (var knowledgeSkillBtn in Buttons[megaKnowledge.TowerSet].Values)
                        {
                            knowledgeSkillBtn.SetState(KnowlegdeSkillBtnState.Available);
                        }

                        foreach (var mkv in ModContent.GetContent<MegaKnowledge>().Where(mkv =>
                                     mkv.TowerSet == megaKnowledge.TowerSet))
                        {
                            if (mkv == megaKnowledge) continue;
                            mkv.Enabled = false;
                        }
                    }

                    if (Input.GetKey(KeyCode.LeftAlt) ||
                        (megaKnowledge.Enabled && knowledgeSkillTree.currSelectedBtn == skillBtn))
                    {
                        megaKnowledge.Enabled = false;
                        skillBtn.SetState(KnowlegdeSkillBtnState.Available);
                    }
                    else
                    {
                        megaKnowledge.Enabled = true;

                        skillBtn.SetState(KnowlegdeSkillBtnState.Purchased);
                    }
                    if (!Input.GetKey(KeyCode.LeftAlt))
                    {
                        knowledgeSkillTree.BtnClicked(skillBtn);
                    }
                    knowledgeSkillTree.selectedPanelTitleTxt.SetText(megaKnowledge.DisplayName);
                    knowledgeSkillTree.selectedPanelDescTxt.SetText(megaKnowledge.Description);
                });
                btn.Construct(newButton);
                if (!Buttons.ContainsKey(megaKnowledge.TowerSet))
                {
                    Buttons[megaKnowledge.TowerSet] = new Dictionary<string, KnowledgeSkillBtn>();
                }

                Buttons[megaKnowledge.TowerSet][megaKnowledge.Name] = btn;
                btn.SetState(megaKnowledge.Enabled
                    ? KnowlegdeSkillBtnState.Purchased
                    : KnowlegdeSkillBtnState.Available);

                var image = btn.GetComponentsInChildren<Component>().First(component => component.name == "Icon")
                    .GetComponent<Image>();

                image.SetSprite(ModContent.GetSpriteReference<MegaKnowledgeMod>(megaKnowledge.Name));
                image.mainTexture.filterMode = FilterMode.Trilinear;
            }

            foreach (var gameObject in knowledgeSkillTree.scrollers)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 500);
            }
        }
    }

    [HarmonyPatch(typeof(KnowledgeSkillTree), nameof(KnowledgeSkillTree.Update))]
    internal class KnowledgeSkillTree_Update
    {
        [HarmonyPostfix]
        internal static void Postfix(KnowledgeSkillTree __instance)
        {
            foreach (var knowledgeSkillBtns in Buttons.Values)
            {
                foreach (var megaKnowledge in knowledgeSkillBtns.Keys)
                {
                    knowledgeSkillBtns[megaKnowledge]
                        .SetSelected(ModContent.GetContent<MegaKnowledge>()
                            .First(knowledge => knowledge.Name == megaKnowledge).Enabled);
                }
            }
        }
    }


    [HarmonyPatch(typeof(KnowledgeMain), nameof(KnowledgeMain.OnEnable))]
    internal class KnowledgeMain_Open
    {
        [HarmonyPostfix]
        internal static void Postfix(KnowledgeMain __instance)
        {
            var texts = new Dictionary<TowerSet, GameObject>
            {
                { Primary, __instance.primaryCompletedTxt.gameObject },
                { Military, __instance.militaryCompletedTxt.gameObject },
                { Magic, __instance.magicCompletedTxt.gameObject },
                { Support, __instance.supportCompletedTxt.gameObject },
            };

            foreach (var (set, text) in texts)
            {
                var button = text.transform.parent.gameObject;
                var existing = button.transform.FindChild(set.ToString());
                if (existing != null)
                {
                    continue;
                }

                var image = button.transform.FindChild("MKIcon").GetComponentInChildren<Image>().gameObject;
                var i = 25;
                foreach (var megaKnowledge in ModContent.GetContent<MegaKnowledge>())
                {
                    if (!megaKnowledge.Enabled || megaKnowledge.TowerSet != set) continue;

                    var newImage = Object.Instantiate(image, button.transform, true);
                    //newImage.transform.Translate(i - 75, -150 - i / 3.6f, 0);
                    newImage.transform.Translate(-150f + i / 3.6f, i, 0);
                    newImage.transform.Rotate(0, 0, -15.5f);
                    newImage.name = set.ToString();

                    var realImage = newImage.GetComponent<Image>();

                    realImage.SetSprite(ModContent.GetSpriteReference<MegaKnowledgeMod>(megaKnowledge.Name));
                    realImage.mainTexture.filterMode = FilterMode.Trilinear;
                    i += 160;
                }
            }
        }
    }
}