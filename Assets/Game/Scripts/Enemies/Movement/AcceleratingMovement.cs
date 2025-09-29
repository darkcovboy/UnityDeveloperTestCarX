using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public class AcceleratingMovement : IMovementStrategy
    {
        private readonly Transform _target;
        private readonly float _acceleration;

        private float _currentSpeed;

        public AcceleratingMovement(Transform target, float initialSpeed, float acceleration)
        {
            _target = target;
            _currentSpeed = initialSpeed;
            _acceleration = acceleration;
        }

        public Vector3 GetVelocity(Transform transform)
        {
            if (_target == null) return Vector3.zero;

            _currentSpeed += _acceleration * Time.deltaTime;

            Vector3 dir = (_target.position - transform.position).normalized;
            return dir * _currentSpeed;
        }

        public bool HasReachedDestination(Transform transform, float reachDistance)
        {
            if (_target == null) return false;
            return Vector3.Distance(transform.position, _target.position) <= reachDistance;
        }

        public Vector3 GetFuturePosition(Transform transform, float time)
        {
            if (_target == null) return transform.position;

            Vector3 dir = (_target.position - transform.position).normalized;

            float distance = _currentSpeed * time + 0.5f * _acceleration * time * time;
            return transform.position + dir * distance;
        }
    }
}