using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public interface IMovementStrategy
    {
        public Vector3 GetVelocity(Transform currentPosition, float speed);
        public bool HasReachedDestination(Transform currentPosition, float reachDistance);
    }
}