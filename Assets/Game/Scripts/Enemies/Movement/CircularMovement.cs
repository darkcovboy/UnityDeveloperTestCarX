using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public class CircularMovement : IMovementStrategy
    {
        private readonly Transform _center;
        private readonly float _speed;
        private readonly float _radius;
        private readonly float _angularSpeed;
        
        private float _angle;
        private bool _reachedCircle = false;
        private float _circleHeight;


        public CircularMovement(Transform center, float speed, float radius, float angularSpeed)
        {
            _center = center;
            _speed = speed;
            _radius = radius;
            _angularSpeed = angularSpeed;
        }

        public Vector3 GetVelocity(Transform transform)
        {
            if (!_reachedCircle)
            {
                Vector3 dirToCenter = (transform.position - _center.position).normalized;
                dirToCenter.y = 0;
                Vector3 circlePoint = _center.position + dirToCenter * _radius;

                circlePoint.y = transform.position.y;
                
                Vector3 toCircle = circlePoint - transform.position;

                if (toCircle.magnitude < 0.1f)
                {
                    _reachedCircle = true;
                    _circleHeight = transform.position.y;

                    Vector3 relative = transform.position - _center.position;
                    _angle = Mathf.Atan2(relative.z, relative.x);
                    return Vector3.zero;
                }

                return toCircle.normalized * _speed;
            }

            _angle += _angularSpeed * Time.deltaTime;

            Vector3 offset = new Vector3(Mathf.Cos(_angle), 0, Mathf.Sin(_angle)) * _radius;
            Vector3 targetPos = _center.position + offset;
            targetPos.y = _circleHeight;

            return (targetPos - transform.position) / Time.deltaTime;
        }

        public bool HasReachedDestination(Transform transform, float reachDistance)
        {
            // у окружности нет "конечной точки"
            return false;
        }

        public Vector3 GetFuturePosition(Transform transform, float t)
        {
            if (!_reachedCircle)
            {
                Vector3 dirToCenter = (transform.position - _center.position).normalized;
                Vector3 circlePoint = _center.position + dirToCenter * _radius;
                return Vector3.Lerp(transform.position, circlePoint, t);
            }

            float futureAngle = _angle + _angularSpeed * t;
            Vector3 offset = new Vector3(Mathf.Cos(futureAngle), 0, Mathf.Sin(futureAngle)) * _radius;
            
            Vector3 targetPos = _center.position + offset;

            return targetPos;
        }
    }
}