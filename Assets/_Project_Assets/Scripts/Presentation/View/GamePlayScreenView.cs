using System.Collections.Generic;
using System.Linq;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Presentation.View
{
    public class GamePlayScreenView : MonoBehaviour
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingsBtn;

        [SerializeField] private List<Toggle> difficultyLevels;

        [SerializeField] private TMP_Text bestScoreTxt;

        private GamePlayScreenController _controller;

        [Inject]
        private void Init(SignalBus signalBus, DifficultyLevelData  difficultyLevelData) =>
            _controller = new GamePlayScreenController(signalBus, difficultyLevelData);

        private void Start()
        {
            startBtn.onClick.AddListener(StartGame);
            startBtn.onClick.AddListener(OpenSettings);
        }

        private void OpenSettings() =>
            _controller.OpenSettings();

        private void StartGame()
        {
            Toggle activeToggle = difficultyLevels.FirstOrDefault(x => x.isOn);
            int index = 0;
            foreach (var toggle in difficultyLevels)
            {
                if (toggle == activeToggle)
                    break;
                index++;
            }

            _controller.StartGame(index);
        }

        private void OnDestroy()
        {
            startBtn.onClick.RemoveListener(StartGame);
            startBtn.onClick.RemoveListener(OpenSettings);
        }
    }
}