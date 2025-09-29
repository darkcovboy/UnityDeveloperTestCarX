using Game.Scripts.Enemies.Health;
using UnityEngine;

namespace Game.Scripts.Towers.Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [SerializeField] private int _damage = 10;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}