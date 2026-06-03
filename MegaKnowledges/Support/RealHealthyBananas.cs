using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Support;

public class RealHealthyBananas : MegaKnowledge
{
    public override string TowerId => TowerType.BananaFarm;
    public override string Description =>
        "Healthy Bananas makes all Banana Farms give 1 life per round per upgrade (aka tier + 1 per round).";
    public override int Offset => 0;

    public override void Apply(TowerModel model)
    {
        var amount = model.tier + 1;
        var bonusLivesPerRoundModel = model.GetBehavior<BonusLivesPerRoundModel>();
        if (bonusLivesPerRoundModel == null)
        {
            model.AddBehavior(BonusLivesPerRoundModel.Create(new()
            {
                name = "BonusLivesPerRoundModel_HealthyBananas",
                amount = amount,
                lifespan = 1.25f,
                assetId = CreatePrefabReference("eb70b6823aec0644c81f873e94cb26cc")
            }));
        }
        else
        {
            bonusLivesPerRoundModel.amount = amount;
        }
    }
}