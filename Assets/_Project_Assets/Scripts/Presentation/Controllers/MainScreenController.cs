using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using UnityEngine;
using Zenject;

namespace Scripts.Presentation.Controllers
{
    public class MainScreenController
    {
        private readonly SignalBus _signalBus;
        private readonly DifficultyLevelData _difficultyLevelData;

        public MainScreenController(SignalBus signalBus, DifficultyLevelData difficultyLevelData)
        {
            _signalBus = signalBus;
            _difficultyLevelData = difficultyLevelData;
        }

        public void StartGame(int index)
        {
            int clampIndex = Mathf.Clamp(index, 0, _difficultyLevelData.GridSizes.Count - 1);
            _signalBus.TryFire(new StartGameSignal(_difficultyLevelData.GridSizes[clampIndex]));
        }

        public void OpenSettings() => 
            _signalBus.TryFire(new OpenSettingsSignal(true));
    }
}