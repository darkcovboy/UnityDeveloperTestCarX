using Game.Scripts.Enemies.Movement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Enemies.Data
{
    [CreateAssetMenu(fileName = "MonsterConfig", menuName = "Configs/Monsters/MonsterConfig", order = 0)]
    public class MonsterConfig : ScriptableObject
    {
        [Title("Movement Settings")]
        [SerializeField, Min(0)] private float _speed = 3f;
        [SerializeField] private float _reachDistance = 0.3f;
        [SerializeField, Min(0)] private float _acceleration = 0.5f;
        [SerializeField, Min(0)] private float _radius = 5f;
        [SerializeField, Min(0)] private float _angularSpeed = 1f;
        [Title("Stats")]
        [SerializeField, Min(1)] private float _maxHealth = 30;
        public float Speed => _speed;
        public float MaxHealth => _maxHealth;
        public float ReachDistance => _reachDistance;
        public float Acceleration => _acceleration;
        
        public float Radius => _radius;
        public float AngularSpeed => _angularSpeed;
    }
}