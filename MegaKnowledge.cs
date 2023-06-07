using System.Collections.Generic;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.TowerSets;
using MelonLoader;
using static Il2CppAssets.Scripts.Models.TowerSets.TowerSet;

namespace MegaKnowledge;

public abstract class MegaKnowledge : NamedModContent
{
    private MelonPreferences_Entry<bool> setting;

    public bool Enabled
    {
        get => setting?.Value == true;
        set => setting.Value = value;
    }

    public abstract string TowerId { get; }

    public abstract int Offset { get; }

    public virtual bool TargetChanging => false;

    public TowerSet towerSet;


    public override void Register()
    {
        MegaKnowledgeMod.MegaKnowledgeCategory ??= MelonPreferences.CreateCategory("MegaKnowledges");

        towerSet = Game.instance.model.GetTowerWithName(TowerId).towerSet;

        setting = MegaKnowledgeMod.MegaKnowledgeCategory.CreateEntry(Name, false, DisplayName, Description);
    }

    public abstract void Apply(TowerModel model);

    public string KnowledgeToCloneFrom => towerSet switch
    {
        Primary => "MoreCash",
        Military => "BigBloonSabotage",
        Magic => "ManaShield",
        Support => "BankDeposits",
        _ => ""
    };

    public virtual void OnUpdate()
    {
    }
}