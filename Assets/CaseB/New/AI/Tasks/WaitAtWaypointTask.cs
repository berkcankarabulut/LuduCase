using CaseB.New.BehaviorTree;
using CaseB.New.Movement;
using UnityEngine;

namespace CaseB.New.AI.Tasks
{
    public class WaitAtWaypointTask : Node
    {
        private WaypointSystem waypointSystem;

        public WaitAtWaypointTask(
            WaypointSystem waypointSystem,
            string name = "Wait At Waypoint"
        ) : base(name)
        {
            this.waypointSystem = waypointSystem;
        }

        public override NodeStatus Execute()
        {
            if (!waypointSystem.IsWaiting())
                return NodeStatus.Success;
 
            bool waitCompleted = waypointSystem.UpdateWait(Time.deltaTime);
             
            return waitCompleted ? NodeStatus.Success : NodeStatus.Running;
        }
    }
}