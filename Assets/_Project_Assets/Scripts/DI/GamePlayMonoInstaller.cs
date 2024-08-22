using Scripts.Data;
using Scripts.Services;
using Scripts.Services.Interfaces;
using Zenject;

namespace Scripts.DI
{
    public class GamePlayMonoInstaller : MonoInstaller
    {
        private readonly IPersistentService _persistentService = new PersistentService();

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

            projContainer.Bind<IPersistentService>().FromInstance(_persistentService).AsSingle();

            ISpawnService spawnService = new SpawnService(projContainer);
            projContainer.Bind<ISpawnService>().FromInstance(spawnService).AsSingle();
        }

        private void SetDefaultData()
        {
            SettingsDto settingsDto = _persistentService.Load<SettingsDto>();
            if (settingsDto == null)
                _persistentService.Save(new SettingsDto(true));

            ScoreDto scoreDto = _persistentService.Load<ScoreDto>();
            if (scoreDto == null)
                _persistentService.Save(new ScoreDto(0));
        }
    }
}