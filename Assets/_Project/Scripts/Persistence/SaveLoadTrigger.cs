using _Project.Scripts.Common.DependencyInjection;
using _Project.Scripts.Common.EventBus;
using DungeonCrawler._Project.Scripts.Events;
using DungeonCrawler._Project.Scripts.Persistence.Events;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Persistence
{
    public class SaveLoadTrigger : MonoBehaviour
    {
        [Inject] private SaveLoadSystem _saveLoadSystem;

        public void SaveCurrentGame()
        {
            if (_saveLoadSystem == null)
                Debug.LogWarning($"[SaveLoadTrigger] Save Game asked");
            else 
                EventBus<SaveGameEvent>.Raise(new SaveGameEvent());
        }
        
        public void NewGame()
        {
            if (_saveLoadSystem == null)
                Debug.LogWarning($"[SaveLoadTrigger] New Game created");
            else
                EventBus<NewGameEvent>.Raise(new NewGameEvent());
        }
        
        
        public void LoadGame()
        {
            if (_saveLoadSystem == null)
                Debug.LogWarning($"[SaveLoadTrigger] Load last Game");
            else
                EventBus<ContinueGameEvent>.Raise(new ContinueGameEvent());
        }
    }
}