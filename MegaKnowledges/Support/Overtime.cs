using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Support;

public class Overtime : MegaKnowledge
{
    public override string TowerId => TowerType.EngineerMonkey;

    public override string Description => MegaKnowledgeMod.OpOvertime
        ? "Engineers and their sentries are permanently overclocked."
        : "Engineers permanently Overclock themselves.";

    public override int Offset => 1200;


    public override void Apply(TowerModel model)
    {
    }

    public override void OnUpdate()
    {
        var overclock = Game.instance.model
            .GetTower(TowerType.EngineerMonkey, 0, 4)
            .GetAbility()
            .GetBehavior<OverclockModel>();
        foreach (var tts in InGame.instance.bridge.GetAllTowers())
        {
            var baseId = tts.tower.towerModel.baseId;
            if ((baseId == TowerType.EngineerMonkey ||
                 baseId.Contains(TowerType.Sentry) && MegaKnowledgeMod.OpOvertime) &&
                tts.tower.GetMutatorById("Overclock") == null)
            {
                tts.tower.AddMutator(overclock.Mutator, -1, false);
            }
        }
    }
}