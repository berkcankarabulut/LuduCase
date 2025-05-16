using UnityEngine;

namespace CaseB.New.Movement
{
    public interface IMovementController
    {
        void SetDestination(Vector3 destination);
        void SetRotationTarget(Vector3 target);
        void Move(float deltaTime);
        void Rotate(float deltaTime);
        bool HasReachedDestination(float arrivalDistance);
        void Stop();
    }
}