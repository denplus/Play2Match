using Scripts.Data.Signals;
using Zenject;

namespace Scripts.DI
{
    public class SignalBusInstall
    {
        public SignalBusInstall(DiContainer container)
        {
            SignalBusInstaller.Install(container);

            container.DeclareSignal<EndGameSignal>();
            container.DeclareSignal<StartGameSignal>();
            container.DeclareSignal<OpenSettingsSignal>();
            container.DeclareSignal<CardFlipSignal>();
            container.DeclareSignal<CardMatchStateSignal>();
        }
    }
}