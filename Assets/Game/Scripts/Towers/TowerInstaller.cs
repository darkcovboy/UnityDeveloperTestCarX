using Game.Scripts.Towers.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Towers
{
    public class TowerInstaller : MonoInstaller
    {
        [SerializeField] private ProjectileCatalog _projectileCatalog;
        
        public override void InstallBindings()
        {
            Container.Bind<ProjectileFactory>()
                .AsSingle()
                .WithArguments(_projectileCatalog.ProjectilePrefabs)
                .NonLazy();
        }
    }
}