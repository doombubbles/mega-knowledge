using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

namespace MegaKnowledge.MegaKnowledges.Magic;

public class Oktoberfest : MegaKnowledge
{
    public override string TowerId => TowerType.Alchemist;
    public override string Description => "Alchemist buff potions last for 50% more shots.";
    public override int Offset => -800;

    public override void Apply(TowerModel model)
    {
        var brew = model.GetDescendant<AddBerserkerBrewToProjectileModel>();
        if (brew != null)
        {
            brew.cap = (int) (brew.cap * 1.5);
            //brew.rebuffBlockTime = 0;
            //brew.rebuffBlockTimeFrames = 0;
            var brewCheck = brew.towerBehaviors[0].Cast<BerserkerBrewCheckModel>();
            brewCheck.maxCount = (int) (brewCheck.maxCount * 1.5);
        }

        var dip = model.GetDescendant<AddAcidicMixtureToProjectileModel>();
        if (dip != null)
        {
            dip.cap = (int) (dip.cap * 1.5);
            //brew.rebuffBlockTime = 0;
            //brew.rebuffBlockTimeFrames = 0;
            var dipCheck = dip.towerBehaviors[0].Cast<AcidicMixtureCheckModel>();
            dipCheck.maxCount = (int) (dipCheck.maxCount * 1.5);
        }
    }
}