using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.Loader
{
    public class GameLauncher
    {
        private const string SceneName = "Gameplay";
        private readonly ISceneLoader _sceneLoader;

        public GameLauncher(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public void Launch()
        {
            Load().Forget();
        }

        private async UniTask Load() => await _sceneLoader.LoadSceneAsync(SceneName);
    }
}