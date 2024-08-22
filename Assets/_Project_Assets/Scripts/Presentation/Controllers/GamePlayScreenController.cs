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

        public GamePlayScreenController(SignalBus signalBus, ISpawnService spawnService, Transform cardHolder, CardImagesData cardImagesData)
        {
            _signalBus = signalBus;
            _spawnService = spawnService;
            _cardHolder = cardHolder;
            _cardImagesData = cardImagesData;
        }

        public void SpawnCards(Vector2 signalGridSize, CardUnitView cardUnitPrefab)
        {
            // get unique images
            
            for (int x = 0; x < signalGridSize.x; x++)
            {
                for (int y = 0; y < signalGridSize.y; y++)
                {
                     CardUnitView card = _spawnService.BindGetUnit(cardUnitPrefab, _cardHolder);
                     card.SetImage(_cardImagesData.CardImages[0]);
                }
            }
        }
    }
}