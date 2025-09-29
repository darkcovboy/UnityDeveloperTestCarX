using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.Loader.Screen
{
    [CreateAssetMenu(
        fileName = "LoadingScreenInstaller",
        menuName = "Zenject/New LoadingScreenInstaller"
    )]
    public class LoaderScreenInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private LoaderScreen _screenPrefab;
        
        public override void InstallBindings()
        {
            this.Container
                .BindInterfacesAndSelfTo<LoaderScreen>()
                .FromComponentInNewPrefab(_screenPrefab)
                .AsSingle()
                .OnInstantiated<LoaderScreen>((_, it) => it.Hide())
                .NonLazy();
        }

    }
}