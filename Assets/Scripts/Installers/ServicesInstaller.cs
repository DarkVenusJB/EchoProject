using GameEntities.Player;
using Services.InputService;
using Services.PlayerSpawnService;
using Services.SaveLoadService;
using UnityEngine;
using Zenject;

public class ServicesInstaller : MonoInstaller
{
    [SerializeField] private GameObject playerPrefab;
    public override void InstallBindings()
    {
       InstallServices();
       InstallPlayer();
    }

    private void InstallServices()
    {
        Container.Bind<InputService>().AsSingle().NonLazy();
        Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle().NonLazy();
    }

    private void InstallPlayer()
    {
    }
}
