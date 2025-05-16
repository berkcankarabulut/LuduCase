using UnityEngine;

namespace CaseB.New.Movement
{
    public class BasicMovementController : IMovementController
    {
        private Transform _transform;
        private Vector3 _destination;
        private Vector3 _rotationTarget;
        private float _moveSpeed;
        private float _rotationSpeed;
        private bool _shouldMove = false;

        public BasicMovementController(Transform transform, float moveSpeed, float rotationSpeed)
        {
            _transform = transform;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _destination = transform.position;
            _rotationTarget = transform.position + transform.forward;
        }

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
            _shouldMove = true;
        }

        public void SetRotationTarget(Vector3 target)
        {
            _rotationTarget = target;
        }

        public void Move(float deltaTime)
        {
            if (!_shouldMove)
                return;

            Vector3 direction = (_destination - _transform.position).normalized;
            _transform.position += direction * (_moveSpeed * deltaTime);
        }

        public void Rotate(float deltaTime)
        {
            Vector3 direction = (_rotationTarget - _transform.position).normalized;
            if (direction == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(
                _transform.rotation,
                targetRotation,
                deltaTime * _rotationSpeed
            );
        }

        public bool HasReachedDestination(float arrivalDistance)
        {
            float distance = Vector3.Distance(_transform.position, _destination);
            return distance <= arrivalDistance;
        }

        public void Stop()
        {
            _shouldMove = false;
        }
    }
}