using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Utils;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;

namespace MegaKnowledge.MegaKnowledges.Magic;

public class ShadowDouble : MegaKnowledge
{
    public override string TowerId => TowerType.NinjaMonkey;
    public override string Description => "Ninja Monkeys can throw extra Shurikens behind them if Bloons are present.";
    public override int Offset => -1200;

    public override void Apply(TowerModel model)
    {
        var attackModel = model.GetAttackModel();
        var weapon = attackModel.weapons[0];
        var newWeapon = weapon.Duplicate();
        newWeapon.projectile.display = CreatePrefabReference<ShadowShuriken>();
        weapon.AddBehavior(new FireAlternateWeaponModel("", 1));

        newWeapon.AddBehavior(new FireWhenAlternateWeaponIsReadyModel("", 1));
        newWeapon.AddBehavior(new FilterTargetAngleFilterModel("", 45.0f, 180f, true,
            56));

        var arcEmissionModel = newWeapon.emission.TryCast<ArcEmissionModel>();
        if (arcEmissionModel != null)
        {
            newWeapon.emission.AddBehavior(new EmissionArcRotationOffTowerDirectionModel("", 180));
        }
        else
        {
            newWeapon.emission.AddBehavior(new EmissionRotationOffTowerDirectionModel("", 180));
        }

        newWeapon.name += " Secondary";
        newWeapon.ejectX *= -1;

        var trackTargetWithinTimeModel = newWeapon.projectile.GetBehavior<TrackTargetWithinTimeModel>();
        if (trackTargetWithinTimeModel != null)
        {
            trackTargetWithinTimeModel.name += "Behind";
        }


        attackModel.AddWeapon(newWeapon);
    }
}
    
public class ShadowShuriken : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.NinjaMonkey).GetWeapon().projectile.display;
        
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, Name);
    }
}