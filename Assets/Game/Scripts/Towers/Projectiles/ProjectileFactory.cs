using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Towers.Projectiles
{
    public class ProjectileFactory
    {
        private readonly Dictionary<ProjectileType, ProjectileBase> _prefabs;

        public ProjectileFactory(Dictionary<ProjectileType, ProjectileBase> prefabs)
        {
            _prefabs = prefabs;
        }

        public ProjectileBase GetProjectile(ProjectileType projectileType, Vector3 position, Quaternion rotation)
        {
            if (!_prefabs.TryGetValue(projectileType, out ProjectileBase _))
            {
                Debug.LogError($"Не зарегестрирован префаб для типа {projectileType}");
                return null;
            }
            
            return Object.Instantiate(_prefabs[projectileType], position, rotation);
        }

        public ProjectileBase GetPrefab(ProjectileType projectileType)
        {
            if (!_prefabs.TryGetValue(projectileType, out ProjectileBase _))
            {
                Debug.LogError($"Не зарегестрирован префаб для типа {projectileType}");
                return null;
            }
            
            return _prefabs[projectileType];
        }
    }
}