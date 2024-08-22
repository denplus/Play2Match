using System.Collections.Generic;
using Scripts.Data.ScriptableObject;
using Scripts.Presentation.View;
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

        private readonly List<CardUnitView> _spawnedCards = new();
        private readonly GamePlayScreenUI _view;

        private CardUnitView _prevCard;
        private int _score;
        private int _attempts;

        public GamePlayScreenController(GamePlayScreenUI view, SignalBus signalBus, ISpawnService spawnService, Transform cardHolder, CardImagesData cardImagesData)
        {
            _signalBus = signalBus;
            _spawnService = spawnService;
            _cardHolder = cardHolder;
            _cardImagesData = cardImagesData;
            _view = view;
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

        private void OnFlipCard(CardUnitView cardUnitView)
        {
            if (_prevCard != null)
            {
                if (_prevCard.Index == cardUnitView.Index)
                {
                    _score++;
                    _view.UpdateScore(_score);
                }
                else
                {
                    _attempts++;
                    _view.UpdateAttempt(_attempts);
                }

                _prevCard = null;
            }
            else
            {
                _prevCard = cardUnitView;
            }
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