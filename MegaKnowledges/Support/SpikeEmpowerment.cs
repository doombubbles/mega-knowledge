using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Powers;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace MegaKnowledge.MegaKnowledges.Support;

public class SpikeEmpowerment : MegaKnowledge
{
    public override string TowerId => TowerType.SpikeFactory;

    public override string Description =>
        "Once per round, Spike Factories emit a permanent Road Spikes power on the track. " +
        "It does extra damage based on the tier of the Spike Factory.";

    public override int Offset => 400;

    public override string DisplayName => "Spikes R Us";

    public override void Apply(TowerModel model)
    {
        var proj = currentGameModel.GetPowerWithId("RoadSpikes")
            .GetBehavior<RoadSpikesModel>().projectileModel
            .Duplicate();

        proj.GetBehavior<DamageModel>().damage += model.tier * model.tier;

        var attackModel = model.GetAttackModel();
        var weapon = attackModel.weapons[0].Duplicate(Name);

        proj.AddBehavior(weapon.GetDescendant<ArriveAtTargetModel>());
        proj.AddBehavior(weapon.GetDescendant<ScaleProjectileModel>());
        proj.AddBehavior(weapon.GetDescendant<HeightOffsetProjectileModel>());

        weapon.SetProjectile(proj);

        weapon.AddBehavior(new EmissionsPerRoundFilterModel("", 1));

        attackModel.AddWeapon(weapon);
    }
}