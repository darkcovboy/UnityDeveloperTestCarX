using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public interface IMovementStrategy
    {
        public Vector3 GetVelocity(Transform currentPosition);
        public bool HasReachedDestination(Transform currentPosition, float reachDistance);
        Vector3 GetFuturePosition(Transform transform, float deltaTime);
    }
}