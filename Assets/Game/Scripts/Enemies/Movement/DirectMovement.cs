using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public class DirectMovement : IMovementStrategy
    {
        private readonly Transform _target;

        public DirectMovement(Transform target)
        {
            _target = target;
        }

        public Vector3 GetVelocity(Transform currentPosition, float speed)
        {
            if (_target == null) return Vector3.zero;

            Vector3 direction = (_target.position - currentPosition.position).normalized;
            return direction * speed;
        }

        public bool HasReachedDestination(Transform currentPosition, float reachDistance)
        {
            if (_target == null) return false;
            return Vector3.Distance(currentPosition.position, _target.position) <= reachDistance;
        }
    }
}