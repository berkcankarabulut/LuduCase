using UnityEngine;
using System.Collections.Generic;
using CaseB.New.AI.Tasks;
using CaseB.New.BehaviorTree;
using CaseB.New.Movement; 

namespace CaseB.New.AI
{
    public class NewEnemyAI : MonoBehaviour
    {
        [Header("Waypoint Settings")] 
        [SerializeField] private List<Transform> _waypoints = new List<Transform>();

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _waypointArrivalDistance = 0.5f;
        [SerializeField] private float _waitTimeAtWaypoints = 1f;
        [SerializeField] private bool _loopWaypoints = true;

        private IMovementController _movementController;
        private WaypointSystem _waypointSystem;

        private Node rootNode;

        private void Awake()
        { 
            _movementController = new BasicMovementController(transform, _moveSpeed, _rotationSpeed);
            _waypointSystem = new WaypointSystem(_waypoints, _loopWaypoints, _waitTimeAtWaypoints);
        }

        private void Start()
        {
            if (_waypoints.Count == 0)
            {
                Debug.LogWarning("No waypoints assigned to " + gameObject.name);
            }
 
            ConstructBehaviorTree();
        }

        private void Update()
        {
            if (_waypoints.Count == 0) return; 
            rootNode.Execute();
        }

        private void ConstructBehaviorTree()
        { 
            rootNode = new Sequence("Waypoint Following")
                .AddChild(new MoveToWaypointTask(_movementController, _waypointSystem, _waypointArrivalDistance))
                .AddChild(new WaitAtWaypointTask(_waypointSystem));
        } 
    }
}