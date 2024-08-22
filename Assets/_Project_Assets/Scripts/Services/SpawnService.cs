using Scripts.Services.Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Scripts.Services
{
    public class SpawnService : ISpawnService
    {
        private readonly DiContainer _container;

        public SpawnService(DiContainer container) => 
            _container = container;

        public T BindGetUnit<T>(T prefab, Transform transform) where T : Object =>
            _container.InstantiatePrefabForComponent<T>(prefab, transform);

        public T BindGetUnit<T>(T prefab, Vector3 position, Quaternion quaternion, Transform parent) where T : Object =>
            _container.InstantiatePrefabForComponent<T>(prefab, position, quaternion, parent);
    }
}