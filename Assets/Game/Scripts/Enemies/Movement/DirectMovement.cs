using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public class DirectMovement : IMovementStrategy
    {
        private readonly Transform _target;
        private readonly float _speed;

        public DirectMovement(Transform target, float speed)
        {
            _target = target;
            _speed = speed;
        }

        public Vector3 GetVelocity(Transform currentPosition)
        {
            if (_target == null) return Vector3.zero;

            Vector3 direction = (_target.position - currentPosition.position).normalized;
            return direction * _speed;
        }

        public bool HasReachedDestination(Transform currentPosition, float reachDistance)
        {
            if (_target == null) return false;
            return Vector3.Distance(currentPosition.position, _target.position) <= reachDistance;
        }

        public Vector3 GetFuturePosition(Transform transform, float deltaTime)
        {
            if (_target == null) return transform.position;

            Vector3 velocity = GetVelocity(transform);
            return transform.position + velocity * deltaTime;
        }
    }
}