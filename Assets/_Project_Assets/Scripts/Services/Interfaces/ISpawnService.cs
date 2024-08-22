using UnityEngine;

namespace Scripts.Services.Interfaces
{
    public interface ISpawnService
    {
        T BindGetUnit<T>(T prefab, Transform transform) where T : Object;
        T BindGetUnit<T>(T prefab, Vector3 position, Quaternion quaternion, Transform parent) where T : Object;
    }
}