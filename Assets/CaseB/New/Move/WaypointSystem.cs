using UnityEngine;
using System.Collections.Generic;

namespace CaseB.New.Movement
{ 
    public class WaypointSystem
    {
        private List<Transform> _waypoints;
        private int _currentWaypointIndex;
        private bool _loopWaypoints;
        private bool _isWaiting;
        private float _waitTimer;
        private float _waitDuration;
        private string _currentState = "Idle";

        public string CurrentState => _currentState;

        public Transform CurrentWaypoint =>
            (_currentWaypointIndex < _waypoints.Count) ? _waypoints[_currentWaypointIndex] : null;

        public bool IsCompleted { get; private set; }

        public WaypointSystem(List<Transform> waypoints, bool loopWaypoints, float waitDuration)
        {
            this._waypoints = waypoints ?? new List<Transform>();
            this._loopWaypoints = loopWaypoints;
            this._waitDuration = waitDuration;
            this._currentWaypointIndex = 0;
            this._isWaiting = false;
            this._waitTimer = 0f;
            this.IsCompleted = false;
        }

        public bool IsWaiting()
        {
            return _isWaiting;
        }

        public void StartWaiting()
        {
            _isWaiting = true;
            _waitTimer = 0f;
            _currentState = "Waiting";
        }

        public bool UpdateWait(float deltaTime)
        {
            if (!_isWaiting)
                return false;

            _waitTimer += deltaTime;
            if (_waitTimer >= _waitDuration)
            {
                MoveToNextWaypoint();
                _isWaiting = false;
                return true;
            }

            return false;
        }

        public void MoveToNextWaypoint()
        {
            _currentWaypointIndex++;
 
            if (_currentWaypointIndex >= _waypoints.Count)
            {
                if (_loopWaypoints)
                {
                    _currentWaypointIndex = 0;
                    _currentState = "Looping";
                }
                else
                {
                    IsCompleted = true;
                    _currentState = "Completed";
                }
            }
            else
            {
                _currentState = "Moving";
            }
        }

        public void SetWaypoints(List<Transform> newWaypoints)
        {
            _waypoints = newWaypoints ?? new List<Transform>();
            _currentWaypointIndex = 0;
            _isWaiting = false;
            _waitTimer = 0f;
            IsCompleted = false;
            _currentState = "New Waypoints";
        }

        public List<Transform> GetWaypoints()
        {
            return _waypoints;
        }

        public int GetCurrentWaypointIndex()
        {
            return _currentWaypointIndex;
        }
    }
}