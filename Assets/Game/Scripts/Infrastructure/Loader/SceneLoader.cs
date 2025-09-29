using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Loader.Screen;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Infrastructure.Loader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ILoaderScreen _loadingScreen;

        public SceneLoader(ILoaderScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            _loadingScreen.Show();

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone)
            {
                await UniTask.Yield();
            }
            
            _loadingScreen.Hide();
        }
    }
}