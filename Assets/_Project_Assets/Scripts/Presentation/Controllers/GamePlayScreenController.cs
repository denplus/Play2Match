using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.Data;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.View;
using Scripts.Services.Interfaces;
using Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Scripts.Presentation.Controllers
{
    public class GamePlayScreenController
    {
        public bool PauseGame { get; set; }

        private readonly SignalBus _signalBus;
        private readonly ISpawnService _spawnService;
        private readonly Transform _cardHolder;
        private readonly CardImagesData _cardImagesData;
        private readonly Transform _inputBlocker;
        private readonly IPersistentService _persistentService;

        private readonly List<CardUnitView> _spawnedCards = new();
        private readonly GamePlayScreenUI _view;

        private CardUnitView _prevCard;
        private int _score;
        private int _allAttempts;
        private int _comboAttempts;

        private float _time;

        private List<int> _uniqueIndexes = new();

        public GamePlayScreenController(GamePlayScreenUI view, SignalBus signalBus, ISpawnService spawnService, Transform cardHolder, CardImagesData cardImagesData,
            Transform inputBlocker, IPersistentService persistentService)
        {
            _signalBus = signalBus;
            _spawnService = spawnService;
            _cardHolder = cardHolder;
            _cardImagesData = cardImagesData;
            _inputBlocker = inputBlocker;
            _view = view;
            _persistentService = persistentService;
        }

        public void ResetState()
        {
            PauseGame = false;
            _time = _view.TimerForRound;
            _score = 0;
            _allAttempts = 0;
            _comboAttempts = 0;
            _prevCard = null;
        }

        public async void SpawnCards(Vector2Int signalGridSize, CardUnitView cardUnitPrefab)
        {
            _uniqueIndexes.Clear();
            _uniqueIndexes = GetRandomIndexes(signalGridSize.x * signalGridSize.y / 2);
            List<int> doubledIndexes = new List<int>();
            doubledIndexes.AddRange(_uniqueIndexes);
            doubledIndexes.AddRange(_uniqueIndexes);
            doubledIndexes.Shuffle();

            _spawnedCards.ForEach(x => x.gameObject.SetActive(false));

            for (int x = 0; x < signalGridSize.x; x++)
            {
                for (int y = 0; y < signalGridSize.y; y++)
                {
                    int gridIndex = x * signalGridSize.y + y;
                    int imageIndex = doubledIndexes[gridIndex];

                    if (_spawnedCards.Count > gridIndex) // use already instantiated cards (like object pool) 
                    {
                        _spawnedCards[gridIndex].gameObject.SetActive(true);
                        _spawnedCards[gridIndex].ResetAllSubscriptions();
                        _spawnedCards[gridIndex].OnFlipCard += OnFlipCard;
                        _spawnedCards[gridIndex].SetData(imageIndex);
                        _spawnedCards[gridIndex].SetImage(_cardImagesData.CardImages[imageIndex]);
                    }
                    else // instantiate new card
                    {
                        CardUnitView card = _spawnService.BindGetUnit(cardUnitPrefab, _cardHolder);
                        card.ResetAllSubscriptions();
                        card.OnFlipCard += OnFlipCard;
                        card.SetData(imageIndex);
                        card.SetImage(_cardImagesData.CardImages[imageIndex]);
                        _spawnedCards.Add(card);
                    }
                }
            }

            await ShowCardOnStart();
        }

        private async UniTask ShowCardOnStart()
        {
            _inputBlocker.gameObject.SetActive(true);

            _spawnedCards.ForEach(x => x.AnimateFlip(true));
            await UniTask.Delay((int)(_view.DelayForImageShow * 2 * 1000));
            _spawnedCards.ForEach(x => x.AnimateFlip(false));

            _inputBlocker.gameObject.SetActive(false);
        }

        private async void OnFlipCard(CardUnitView cardUnitView)
        {
            _inputBlocker.gameObject.SetActive(true);

            _signalBus.TryFire(new CardFlipSignal());

            if (_prevCard == null) // first card selected
            {
                _prevCard = cardUnitView;
            }
            else // second card selected
            {
                _allAttempts++;

                bool rightAnswer = _prevCard.Index == cardUnitView.Index;
                if (rightAnswer)
                {
                    _score += 1 + _comboAttempts;
                    _comboAttempts++;
                }
                else
                {
                    _comboAttempts = 0;
                }

                _view.UpdateAttempt(_allAttempts);
                _view.UpdateScore(_score);
                _view.UpdateCombo(_comboAttempts + 1);

                if (rightAnswer) // right match
                {
                    await UniTask.Delay((int)(_view.DelayForImageShow * 1000));

                    _signalBus.TryFire(new CardMatchStateSignal(true));

                    _prevCard.gameObject.SetActive(false);
                    cardUnitView.gameObject.SetActive(false);

                    _uniqueIndexes.Remove(cardUnitView.Index);
                }
                else // wrong match
                {
                    await UniTask.Delay((int)(_view.DelayForImageShow * 1000));

                    _signalBus.TryFire(new CardMatchStateSignal(false));

                    _prevCard.AnimateFlip(false);
                    cardUnitView.AnimateFlip(false);
                }

                _prevCard = null;
            }

            if (_uniqueIndexes.Count == 0) // all cards is match, fire end game signal
                _view.EndGame();

            _inputBlocker.gameObject.SetActive(false);
        }

        public void GameOver()
        {
            ScoreDto prevScore = _persistentService.Load<ScoreDto>();
            if (prevScore.BestScore < _score)
                _persistentService.Save(new ScoreDto(_score));

            _signalBus.TryFire(new EndGameSignal(_score, true));
        }

        public IEnumerator TickTimer()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);
            while (_time > 0)
            {
                if (PauseGame == false)
                {
                    _time--;
                    _view.UpdateTimer((int)_time);
                }

                yield return wait;
            }
            
            _view.EndGame();
        }

        private List<int> GetRandomIndexes(int size)
        {
            int count = _cardImagesData.CardImages.Count;
            List<int> indexes = new List<int>(count);
            for (int i = 0; i < count; i++)
                indexes.Add(i);

            return ListUtil.GetUniqueElementsList(indexes, size);
        }
    }
}