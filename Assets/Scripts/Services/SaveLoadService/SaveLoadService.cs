using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services.SaveLoadService
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly string _savePath = Application.persistentDataPath;

        public async UniTask SaveAsync<T>(string fileName, T data)
        {
            string path = Path.Combine(_savePath, fileName + ".json");
            string json = JsonUtility.ToJson(data, prettyPrint: true);

            using (StreamWriter writer = new StreamWriter(path))
            {
                await writer.WriteAsync(json);
            }
        }

        public async UniTask<T> LoadAsync<T>(string fileName)
        {
            string path = Path.Combine(_savePath, fileName + ".json");

            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл {fileName} не найден!");

            using (StreamReader reader = new StreamReader(path))
            {
                string json = await reader.ReadToEndAsync();
                return JsonUtility.FromJson<T>(json);
            }
        }

        public bool Exists(string fileName)
        {
            string path = Path.Combine(_savePath, fileName + ".json");
            return File.Exists(path);
        }
    }
}