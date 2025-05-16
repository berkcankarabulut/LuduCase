using CaseB.New.BehaviorTree;
using CaseB.New.Movement;
using UnityEngine; 

namespace CaseB.New.AI.Tasks
{ 
    public class MoveToWaypointTask : Node
    {
        private IMovementController movementController;
        private WaypointSystem waypointSystem;
        private float arrivalDistance;

        public MoveToWaypointTask(
            IMovementController movementController,
            WaypointSystem waypointSystem,
            float arrivalDistance,
            string name = "Move To Waypoint"
        ) : base(name)
        {
            this.movementController = movementController;
            this.waypointSystem = waypointSystem;
            this.arrivalDistance = arrivalDistance;
        }

        public override NodeStatus Execute()
        { 
            if (waypointSystem.IsWaiting())
                return NodeStatus.Success;
 
            Transform currentWaypoint = waypointSystem.CurrentWaypoint;
             
            if (currentWaypoint == null)
                return NodeStatus.Failure;
 
            movementController.SetDestination(currentWaypoint.position);
            movementController.SetRotationTarget(currentWaypoint.position);
             
            movementController.Move(Time.deltaTime);
            movementController.Rotate(Time.deltaTime);

            if (!movementController.HasReachedDestination(arrivalDistance)) return NodeStatus.Running;
            waypointSystem.StartWaiting();
            return NodeStatus.Success;

        }
    } 
}