
using System;

namespace DungeonCrawler._Project.Scripts.SceneManagement
{
    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed = (_) => {};

        const float Ratio = 1f;

        public void Report(float value)
        {
            Progressed?.Invoke(value / Ratio);
        }
    }
}
