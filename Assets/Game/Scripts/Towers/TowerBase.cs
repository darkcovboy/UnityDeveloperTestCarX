using System;
using System.Collections.Generic;
using Game.Scripts.Enemies;
using Game.Scripts.Towers.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Towers
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class TowerBase : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float _shootInterval = 0.5f;
        [SerializeField, Min(0.1f)] private float _range = 4f;
        
        protected ProjectileFactory Factory { get; private set; }

        private float _lastShotTime = -1f;
        private readonly List<Monster> _targetsInRange = new();

        [Inject]
        private void Constructor(ProjectileFactory factory)
        {
            Factory = factory;
        }

        private void Awake()
        {
            SetupTriggerCollider();
        }

        private void Update()
        {
            if(_targetsInRange.Count == 0) return;
            
            Monster target = GetNearestTarget();
            if (target == null) return;
            
            Rotate(target);

            if (Time.time >= _lastShotTime + _shootInterval && CanShoot(target))
            {
                Shoot(target);
                _lastShotTime = Time.time;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Monster monster))
            {
                if(!_targetsInRange.Contains(monster))
                    _targetsInRange.Add(monster);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Monster monster))
            {
                if(_targetsInRange.Contains(monster))
                    _targetsInRange.Remove(monster);
            }
        }

        protected abstract void Shoot(Monster target);
        protected virtual bool CanShoot(Monster target) => true;

        protected virtual void Rotate(Monster target)
        {
            
        }

        private Monster GetNearestTarget()
        {
            Monster nearest = null;
            float minDistance = float.MaxValue;

            foreach (var monster in _targetsInRange)
            {
                if (monster == null) continue;

                float distance = Vector3.Distance(transform.position, monster.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = monster;
                }
            }
            
            return nearest;
        }

        private void SetupTriggerCollider()
        {
            SphereCollider sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = _range;
            sphereCollider.isTrigger = true;
        }
    }
}