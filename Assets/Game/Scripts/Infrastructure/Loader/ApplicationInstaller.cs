using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.Loader
{
    [CreateAssetMenu(
        fileName = "ApplicationInstaller",
        menuName = "Zenject/New ApplicationInstaller"
    )]
    public class ApplicationInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneLoader>()
                .AsSingle();   
            
            Container.BindInterfacesAndSelfTo<GameLauncher>()
                .AsSingle()
                .OnInstantiated<GameLauncher>((_, launcher) => launcher.Launch())
                .NonLazy();
        }
    }
}