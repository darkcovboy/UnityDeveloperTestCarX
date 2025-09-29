using System;
using UnityEngine;

namespace Game.Scripts.Towers.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsProjectile : ProjectileBase
    {
        [SerializeField] private Rigidbody _rigidbody;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
        }
#endif

        public void Launch(Vector3 velocity)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = velocity;
        }

    }
}