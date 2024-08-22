using Cysharp.Threading.Tasks;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.Controllers;
using Scripts.Services.Interfaces;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Presentation.View
{
    public class GamePlayScreenUI : BaseUI
    {
        [field: SerializeField] public float DelayForImageShow { get; private set; } = 1f;

        [Header("Buttons"), SerializeField] private Button pauseBtn;
        [SerializeField] private Button backBtn;

        [Header("Cards"), SerializeField] private FlexibleGridLayout flexibleGridLayout;
        [SerializeField] private CardUnitView cardUnitPrefab;
        [SerializeField] private Transform cardHolder;

        [Header("Input blocker"), SerializeField]
        private Transform inputBlocker;

        [Header("Text"), SerializeField] private TMP_Text attemptTxt;
        [SerializeField] private TMP_Text scoreTxt;
        [SerializeField] private TMP_Text comboTxt;

        private SignalBus _signalBus;
        private GamePlayScreenController _controller;

        [Inject]
        private void Init(SignalBus signalBus, ISpawnService spawnService, CardImagesData cardImagesData, IPersistentService persistentService)
        {
            _signalBus = signalBus;
            _controller = new GamePlayScreenController(this, signalBus, spawnService, cardHolder, cardImagesData, inputBlocker, persistentService);
        }

        private void Start()
        {
            pauseBtn.onClick.AddListener(PauseGame);
            backBtn.onClick.AddListener(EndGame);

            _signalBus.Subscribe<StartGameSignal>(OnStartGame);

            UpdateScore(0);
            UpdateAttempt(0);
            UpdateCombo(1);
        }

        public void EndGame()
        {
            _controller.GameOver();
            flexibleGridLayout.enabled = true;
            Hide();
        }

        private void PauseGame()
        {
            
        }

        public void UpdateScore(int attempt) =>
            scoreTxt.text = $"{TextConstance.Score}: {attempt}";

        public void UpdateAttempt(int score) =>
            attemptTxt.text = $"{TextConstance.Attempt}: {score}";
        
        public void UpdateCombo(int combo) =>
            comboTxt.text = $"{TextConstance.Combo}: x{combo}";

        private async void OnStartGame(StartGameSignal signal)
        {
            flexibleGridLayout.AmountX = signal.GridSize.x;
            flexibleGridLayout.AmountY = signal.GridSize.y;

            _controller.ResetState();
            _controller.SpawnCards(signal.GridSize, cardUnitPrefab);

            UpdateScore(0);
            UpdateAttempt(0);
            UpdateCombo(1);

            Show();

            await UniTask.Delay(200); // wait and disable grid layout setter
            flexibleGridLayout.enabled = false;
        }

        private void OnDestroy()
        {
            pauseBtn.onClick.RemoveListener(PauseGame);
            backBtn.onClick.RemoveListener(EndGame);

            _signalBus.TryUnsubscribe<StartGameSignal>(OnStartGame);
        }
    }
}