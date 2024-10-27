using System.Threading.Tasks;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using UnityEngine;
using UnityEngine.UI;
using Scene = UnityEngine.SceneManagement.Scene;

namespace DungeonCrawler._Project.Scripts.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image _loadingBar;
        [SerializeField] private float _fillSpeed = 0.5f;
        [SerializeField] private Canvas _loadingCanvas;
        [SerializeField] private Camera _loadingCamera;
        [SerializeField] private SceneGroup[] _sceneGroups ;

        private float _targetProgress;
        private bool _isLoading;

        public readonly SceneGroupManager manager = new SceneGroupManager();

        private void Awake()
        {
            manager.OnSceneLoaded += HandleSceneLoaded;
            manager.OnSceneUnloaded += scene => Debug.Log($"Unloaded: {scene.name}");
            manager.OnSceneGroupLoaded += () => Debug.Log($"Scene group loaded");
        }

        private void HandleSceneLoaded(Scene scene)
        {
            EventBus<SceneLoadedEvent>.Raise(new SceneLoadedEvent(scene));
        }

        async void Start()
        {
            await LoadSceneGroup(0);
        }

        void Update()
        {
            if (!_isLoading) return;
            float currentFillAmount = _loadingBar.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            float dynamicFillSpeed = progressDifference * _fillSpeed;

            _loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        public async Task LoadSceneGroup(int index)
        {
            _loadingBar.fillAmount = 0f;
            _targetProgress = 1f;

            if (index < 0 || index >= _sceneGroups.Length)
            {
                Debug.LogError($"Invalid Scene group index: {index}");
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);
            
            EnableLoadingCanvas();
            await manager.LoadScenes(_sceneGroups[index], progress);
            EnableLoadingCanvas(false);
        }

        void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            _loadingCanvas.gameObject.SetActive(enable);
            _loadingCamera.gameObject.SetActive(enable);
        }
    }
}