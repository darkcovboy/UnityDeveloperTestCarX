using System;
using UnityEngine;

namespace Game.Scripts.Towers.Projectiles
{
    public class KinematicProjectile : ProjectileBase
    {
        [SerializeField] private float _speed;
        
        public float Speed  => _speed;
        private void Update()
        {
            var translation = transform.forward * _speed *Time.deltaTime;
            transform.Translate (translation,Space.World);
        }
    }
}