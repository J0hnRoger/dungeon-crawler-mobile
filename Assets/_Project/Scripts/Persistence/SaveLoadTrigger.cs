using _Project.Scripts.Common.DependencyInjection;
using UnityEngine;

namespace _Project.Scripts.Persistence
{
    public class SaveLoadTrigger : MonoBehaviour
    {
        [Inject] private SaveLoadSystem _saveLoadSystem;

        public void NewGame()
        {
            if (_saveLoadSystem == null)
                Debug.LogWarning($"[SaveLoadTrigger] New Game created");
            else
                _saveLoadSystem.NewGame();
        }
    }
}