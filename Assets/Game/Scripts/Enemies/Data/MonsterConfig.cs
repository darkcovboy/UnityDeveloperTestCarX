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
        [Title("Stats")]
        [SerializeField, Min(1)] private float _maxHealth = 30;
        public float Speed => _speed;
        public float MaxHealth => _maxHealth;
        public float ReachDistance => _reachDistance;
    }
}