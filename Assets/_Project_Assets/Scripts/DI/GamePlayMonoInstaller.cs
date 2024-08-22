using Scripts.Services;
using Scripts.Services.Interfaces;
using Scripts.Services.Services.Interfaces;
using Zenject;

namespace Scripts.DI
{
    public class GamePlayMonoInstaller : MonoInstaller
    {
         public override void InstallBindings()
        {
            SetDefaultData();
            InitServices();
        }

        private void InitServices()
        {
            DiContainer projContainer = ProjectContext.Instance.Container;
            SignalBusInstall _ = new SignalBusInstall(projContainer);
            SignalBus signalBus = projContainer.Resolve<SignalBus>();

            projContainer.Bind<IPersistentService>().To<PersistentService>().AsSingle();
            projContainer.Bind<ITimeService>().To<TimeService>().AsSingle();
            
            ISpawnService spawnService = new SpawnService(projContainer);
            projContainer.Bind<ISpawnService>().FromInstance(spawnService).AsSingle();
        }

        private void SetDefaultData()
        {
            
           
        }
    }
}
