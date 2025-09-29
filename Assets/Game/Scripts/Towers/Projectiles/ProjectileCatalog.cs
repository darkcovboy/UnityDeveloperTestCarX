using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Scripts.Towers.Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileCatalog", menuName = "Configs/ProjectileCatalog", order = 0)]
    public class ProjectileCatalog : SerializedScriptableObject
    {
        [SerializeField,OdinSerialize] private Dictionary<ProjectileType, ProjectileBase> _projectilePrefabs;
        
        public Dictionary<ProjectileType, ProjectileBase> ProjectilePrefabs => _projectilePrefabs;
    }
}