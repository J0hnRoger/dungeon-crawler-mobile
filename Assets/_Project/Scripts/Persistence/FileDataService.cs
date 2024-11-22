using System.Collections.Generic;
using System.IO;
using DungeonCrawler._Project.Scripts.Persistence;
using UnityEngine;

namespace _Project.Scripts.Persistence
{
    public class FileDataService : IDataService
    {
        private ISerializer _serializer;
        private string _dataPath;
        private string _fileExtension;

        public FileDataService(ISerializer serializer)
        {
            _serializer = serializer;
            _dataPath = Application.persistentDataPath;
            _fileExtension = "json";
        }

        string GetPathToFile(string fileName)
        {
            return Path.Combine(_dataPath, string.Concat(fileName, ".", _fileExtension));
        }

        public void Save(GameData data, bool overwrite = true)
        {
            string fileLocation = GetPathToFile(data.Name);

            if (!overwrite && File.Exists(fileLocation))
                throw new IOException($"The file {fileLocation} already exists and cannot be overwrite");

            File.WriteAllText(fileLocation, _serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (!File.Exists(fileLocation))
                throw new IOException($"No data with name {name}");

            return _serializer.Deserialize<GameData>(fileLocation);
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);
            if (File.Exists(fileLocation))
                File.Delete(fileLocation);
        }

        public void DeleteAll()
        {
            foreach (string file in Directory.GetFiles(_dataPath))
                if (Path.GetExtension(file) == _fileExtension)
                    File.Delete(file);
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string file in Directory.GetFiles(_dataPath))
                if (Path.GetExtension(file) == _fileExtension)
                    yield return Path.GetFileNameWithoutExtension(file);
        }
    }
}