using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Support
{
    public class DigitalAmplification : MegaKnowledge
    {
        public override string TowerId => TowerType.MonkeyVillage;
        public override string Description => "Monkey Villages have greatly increased range.";
        public override int Offset => 800;

        public override void Apply(TowerModel model)
        {
            model.range *= 2;

            foreach (var attackModel in model.GetAttackModels())
            {
                attackModel.range *= 2;
            }
        }
    }
}