using UnityEngine;

namespace Game.Scripts.Infrastructure.Loader.Screen
{
    public class LoaderScreen : MonoBehaviour, ILoaderScreen
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}