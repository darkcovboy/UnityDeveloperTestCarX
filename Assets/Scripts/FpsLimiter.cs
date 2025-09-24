using UnityEngine;

namespace DefaultNamespace
{
    public class FpsLimiter : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;

        void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0;
        }

    }
}