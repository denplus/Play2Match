using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.Data;
using Scripts.Data.ScriptableObject;
using Scripts.Data.Signals;
using Scripts.Presentation.View;
using Scripts.Services.Interfaces;
using Scripts.Services.Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Scripts.Presentation.Controllers
{
    public class GamePlayScreenController
    {
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
        private int _attempts;

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
            _score = 0;
            _attempts = 0;
            _prevCard = null;
        }

        public void SpawnCards(Vector2Int signalGridSize, CardUnitView cardUnitPrefab)
        {
            List<int> uniqueIndexes = GetRandomIndexes(signalGridSize.x * signalGridSize.y / 2);
            List<int> allIndexes = new List<int>();
            allIndexes.AddRange(uniqueIndexes);
            allIndexes.AddRange(uniqueIndexes);

            _spawnedCards.ForEach(x => x.gameObject.SetActive(false));

            for (int x = 0; x < signalGridSize.x; x++)
            {
                for (int y = 0; y < signalGridSize.y; y++)
                {
                    int gridIndex = x * signalGridSize.y + y;
                    int imageIndex = allIndexes[gridIndex];

                    if (_spawnedCards.Count > gridIndex)
                    {
                        _spawnedCards[gridIndex].gameObject.SetActive(true);
                        _spawnedCards[gridIndex].ResetAllSubscriptions();
                        _spawnedCards[gridIndex].OnFlipCard += OnFlipCard;
                        _spawnedCards[gridIndex].SetData(imageIndex);
                        _spawnedCards[gridIndex].SetImage(_cardImagesData.CardImages[imageIndex]);
                    }
                    else
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
        }

        private async void OnFlipCard(CardUnitView cardUnitView)
        {
            _inputBlocker.gameObject.SetActive(true);

            _signalBus.TryFire(new CardFlipSignal());
            
            if (_prevCard != null)
            {
                _attempts++;
                _view.UpdateAttempt(_attempts);

                if (_prevCard.Index == cardUnitView.Index)
                {
                    _score++;
                    _view.UpdateScore(_score);

                    await UniTask.Delay((int)(_view.DelayForImageShow * 1000));
                    
                    _signalBus.TryFire(new CardMatchStateSignal(true));

                    _prevCard.gameObject.SetActive(false);
                    cardUnitView.gameObject.SetActive(false);
                }
                else
                {
                    await UniTask.Delay((int)(_view.DelayForImageShow * 1000));
                    
                    _signalBus.TryFire(new CardMatchStateSignal(false));

                    _prevCard.FlipBack();
                    cardUnitView.FlipBack();

                    await UniTask.Delay((int)(cardUnitView.AnimationDuration * 1000)); // wait for animation to finish
                }

                _prevCard = null;
            }
            else
            {
                _prevCard = cardUnitView;
            }

            _inputBlocker.gameObject.SetActive(false);
        }

        public void EndGame()
        {
            ScoreDto prevScore = _persistentService.Load<ScoreDto>();
            if (prevScore.BestScore < _score)
                _persistentService.Save(new ScoreDto(_score));

            _signalBus.TryFire(new EndGameSignal(_score, true));
        }

        private List<int> GetRandomIndexes(int size)
        {
            int count = _cardImagesData.CardImages.Count;
            List<int> indexes = new List<int>(count);
            for (int i = 0; i < count; i++)
                indexes.Add(i);
            return Utils.ListUtil.GetUniqueElementsList(indexes, size);
        }
    }
}