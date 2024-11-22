using System.Collections.Generic;
using DungeonCrawler._Project.Scripts.Persistence;

namespace _Project.Scripts.Persistence
{
    public interface IDataService
    {
        void Save(GameData data, bool overwrite = true);
        GameData Load(string name);
        void Delete(string name);
        void DeleteAll();
        IEnumerable<string> ListSaves();
    }
}