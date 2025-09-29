using Game.Scripts.Enemies.Data;
using UnityEngine;

namespace Game.Scripts.Enemies.Movement
{
    public class MonsterMovement : MonoBehaviour
    {
        private MonsterConfig _config;
        private IMovementStrategy _movementStrategy;

        public Vector3 Velocity { get; private set; }
        public float Speed => _config.Speed;

        public void Initialize(MonsterConfig config, IMovementStrategy movementStrategy)
        {
            _config = config;
            _movementStrategy = movementStrategy;
        }

        private void Update()
        {
            if (_movementStrategy == null) return;

            Velocity = _movementStrategy.GetVelocity(transform);
            transform.position += Velocity * Time.deltaTime;

            if (_movementStrategy.HasReachedDestination(transform, _config.ReachDistance))
            {
                Destroy(gameObject);
            }
        }

        public Vector3 GetFuturePosition(float t) => _movementStrategy.GetFuturePosition(transform, t);
    }
}