using Cysharp.Threading.Tasks;

namespace Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        UniTask SaveAsync<T>(string fileName, T data);
        UniTask<T> LoadAsync<T>(string fileName);
        bool Exists(string fileName);
    }
}