using UnityEngine;

namespace Game.Scripts.Enemies.Health
{
    public class MonsterHealth : MonoBehaviour, IDamageable
    {
        private float _currentHp;

        public void Initialize(float maxHp)
        {
            _currentHp = maxHp;
        }

        public void ApplyDamage(float damage)
        {
            _currentHp -= damage;
            if (_currentHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public float CurrentHp => _currentHp;
    }
}