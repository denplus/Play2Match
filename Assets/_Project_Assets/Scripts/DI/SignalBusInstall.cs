using Scripts.Data.Signals;
using Zenject;

namespace Scripts.DI
{
    public class SignalBusInstall
    {
        public SignalBusInstall(DiContainer container)
        {
            SignalBusInstaller.Install(container);

            container.DeclareSignal<PlayerEndGameSignal>();
        }
    }
}