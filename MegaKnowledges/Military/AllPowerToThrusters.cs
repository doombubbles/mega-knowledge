using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;

namespace MegaKnowledge.MegaKnowledges.Military
{
    public class AllPowerToThrusters : MegaKnowledge
    {
        public override string TowerId => TowerType.HeliPilot;
        public override string Description => "Heli Pilots can move at hyper-sonic speeds";
        public override int Offset => 400;

        public override void Apply(TowerModel model)
        {
            var heliMovementModel = model.GetDescendant<HeliMovementModel>();

            const int factor = 3;
            heliMovementModel.maxSpeed *= factor;
            heliMovementModel.brakeForce *= factor;
            heliMovementModel.movementForceStart *= factor;
            heliMovementModel.movementForceEnd *= factor;
            heliMovementModel.movementForceEndSquared =
                heliMovementModel.movementForceEnd * heliMovementModel.movementForceEnd;
            heliMovementModel.strafeDistance *= factor;
            heliMovementModel.strafeDistanceSquared =
                heliMovementModel.strafeDistance * heliMovementModel.strafeDistance;
        }
    }
}