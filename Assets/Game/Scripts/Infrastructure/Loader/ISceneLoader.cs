using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.Loader
{
    public interface ISceneLoader
    {
        UniTask  LoadSceneAsync(string sceneName);
    }
}