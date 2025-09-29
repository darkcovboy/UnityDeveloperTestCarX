using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public class WaypointMovement : IMovementStrategy
    {
        private readonly Transform[] _waypoints;
        private readonly float _speed;

        private int _currentIndex = 0;

        public WaypointMovement(Transform[] waypoints, float speed)
        {
            _waypoints = waypoints;
            _speed = speed;
        }

        public Vector3 GetVelocity(Transform transform)
        {
            if (_currentIndex >= _waypoints.Length)
                return Vector3.zero;

            Vector3 target = _waypoints[_currentIndex].position;
            Vector3 toTarget = target - transform.position;

            if (toTarget.magnitude < 0.1f)
            {
                _currentIndex++;
                if (_currentIndex >= _waypoints.Length)
                    return Vector3.zero;

                target = _waypoints[_currentIndex].position;
                toTarget = target - transform.position;
            }

            return toTarget.normalized * _speed;
        }

        public bool HasReachedDestination(Transform transform, float reachDistance)
        {
            return _currentIndex >= _waypoints.Length;
        }

        public Vector3 GetFuturePosition(Transform transform, float t)
        {
            if (_currentIndex >= _waypoints.Length)
                return transform.position;

            Vector3 pos = transform.position;
            float remainingTime = t;

            int idx = _currentIndex;
            while (remainingTime > 0 && idx < _waypoints.Length)
            {
                Vector3 target = _waypoints[idx].position;
                Vector3 toTarget = target - pos;
                float distanceToTarget = toTarget.magnitude;

                float timeToTarget = distanceToTarget / _speed;
                if (remainingTime >= timeToTarget)
                {
                    pos = target;
                    idx++;
                    remainingTime -= timeToTarget;
                }
                else
                {
                    pos += toTarget.normalized * (_speed * remainingTime);
                    remainingTime = 0;
                }
            }

            return pos;
        }
    }
}