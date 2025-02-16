using GameEntities.Player;
using Services.InputService;
using Zenject;

namespace Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputService>().AsSingle().NonLazy();
            Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle();
        }
        
        
    }
}