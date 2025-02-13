using Services.InputService;
using Services.SaveLoadService;
using Zenject;

public class ServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
       InstallServices();
    }

    private void InstallServices()
    {
        Container.Bind<InputService>().AsSingle().NonLazy();
        Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle().NonLazy();
    }
}
