using Game.Scripts.Enemies;
using Game.Scripts.Towers.Projectiles;
using UnityEngine;

namespace Game.Scripts.Towers
{
    public class CannonTower : TowerBase
    {
        private const float AllowedFireAngle = 5f;

        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _rotationSpeed = 180f;
        [SerializeField] private Transform _cannon;
        [SerializeField] private Transform _hub;
        [SerializeField] private float _parabolicAngle = 45f;
        [SerializeField] private FireMode _fireMode = FireMode.Direct;


        protected override void Shoot(Monster target)
        {
            switch (_fireMode)
            {
                case FireMode.Direct:
                    ShootDirect();
                    break;
                case FireMode.Parabolic:
                    ShootParabolic(target);
                    break;
            }
        }

        protected override bool CanShoot(Monster target)
        {
            if (_fireMode == FireMode.Direct)
            {
                Vector3 aimPoint = BallisticMath.GetPredictedPosition(
                    _shootPoint.position,
                    target.transform.position,
                    target.Movement.Velocity,
                    ((KinematicProjectile)Factory.GetPrefab(ProjectileType.Kinematic)).Speed
                );

                Vector3 weaponToAimDirection = (aimPoint - _shootPoint.position).normalized;
                float currentAngle = Vector3.Angle(_cannon.forward, weaponToAimDirection);

                return currentAngle < AllowedFireAngle;
            }

            return true;
        }

        protected override void Rotate(Monster target)
        {
            switch (_fireMode)
            {
                case FireMode.Direct:
                    RotateDirect(target);
                    break;
                case FireMode.Parabolic:
                    RotateParabolic(target);
                    break;
            }
        }

        private void ShootDirect()
        {
            var projectileObj =Factory.GetProjectile(ProjectileType.Kinematic, _shootPoint.position, Quaternion.identity);
            projectileObj.gameObject.transform.rotation = _shootPoint.rotation;
        }
    
        private void RotateDirect(Monster target)
        {
            Vector3 aimPoint = BallisticMath.GetPredictedPosition(_shootPoint.position, target.transform.position,
                target.Movement.Velocity, ((KinematicProjectile)Factory.GetPrefab(ProjectileType.Kinematic)).Speed);

            Vector3 direction = (aimPoint - _shootPoint.position).normalized;

            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
            if (flatDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotY = Quaternion.LookRotation(flatDirection);
                _hub.rotation = Quaternion.RotateTowards(
                    _hub.rotation,
                    targetRotY,
                    _rotationSpeed * Time.deltaTime
                );
            }

            Vector3 localDirection = _hub.InverseTransformDirection(direction);
            Quaternion targetRotX = Quaternion.LookRotation(localDirection);
            _cannon.localRotation = Quaternion.RotateTowards(
                _cannon.localRotation,
                targetRotX,
                _rotationSpeed * Time.deltaTime
            );
        }

        private void RotateParabolic(Monster target)
        {
            float angleRad = _parabolicAngle * Mathf.Deg2Rad;

            float launchSpeed =
                BallisticMath.CalculateLaunchSpeed(_shootPoint.position, target.transform.position, angleRad);
            if (launchSpeed <= 0f) return;

            Vector3 toTargetXZ = new Vector3(
                target.transform.position.x - _shootPoint.position.x,
                0,
                target.transform.position.z - _shootPoint.position.z);

            float x = toTargetXZ.magnitude;
            float t = x / (launchSpeed * Mathf.Cos(angleRad));

            Vector3 predicted = target.transform.position + target.Movement.Velocity * t;

            Vector3 flatDirection = new Vector3(predicted.x - transform.position.x, 0, predicted.z - transform.position.z);
            if (flatDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotY = Quaternion.LookRotation(flatDirection);
                _hub.rotation = Quaternion.RotateTowards(
                    _hub.rotation,
                    targetRotY,
                    _rotationSpeed * Time.deltaTime
                );
            }

            Quaternion targetRotX = Quaternion.Euler(-_parabolicAngle, 0, 0);
            _cannon.localRotation = Quaternion.RotateTowards(
                _cannon.localRotation,
                targetRotX,
                _rotationSpeed * Time.deltaTime
            );
        }

        private void ShootParabolic(Monster target)
        {
            float angleRad = _parabolicAngle * Mathf.Deg2Rad;

            float launchSpeed =
                BallisticMath.CalculateLaunchSpeed(_shootPoint.position, target.transform.position, angleRad);
            if (launchSpeed <= 0f) return;

            Vector3 toTargetXZ = new Vector3(
                target.transform.position.x - _shootPoint.position.x,
                0,
                target.transform.position.z - _shootPoint.position.z);

            float x = toTargetXZ.magnitude;
            float t = x / (launchSpeed * Mathf.Cos(angleRad));

            Vector3 predicted = target.transform.position + target.Movement.Velocity * t;
            
            launchSpeed = BallisticMath.CalculateLaunchSpeed(_shootPoint.position, predicted, angleRad);
            if (launchSpeed <= 0f) return;

            var projectileObj = Factory.GetProjectile(ProjectileType.Physics, _shootPoint.position, Quaternion.identity);
            var projectile = (PhysicsProjectile)projectileObj;

            Vector3 dirXZ = new Vector3(predicted.x - _shootPoint.position.x, 0, predicted.z - _shootPoint.position.z)
                .normalized;

            Vector3 velocity = dirXZ * (launchSpeed * Mathf.Cos(angleRad));
            velocity.y = launchSpeed * Mathf.Sin(angleRad);

            projectile.Launch(velocity);
        }

        public enum FireMode
        {
            Direct,
            Parabolic
        }
    }
}