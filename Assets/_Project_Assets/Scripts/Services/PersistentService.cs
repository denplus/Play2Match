using Newtonsoft.Json;
using Scripts.Services.Interfaces;
using UnityEngine;

namespace Scripts.Services
{
    public class PersistentService : IPersistentService
    {
        private string SavedData<T>() =>
            "saved_data_" + typeof(T);

        public T Load<T>() where T : class
        {
            if (PlayerPrefs.HasKey(SavedData<T>()))
                return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(SavedData<T>()));

            return null;
        }

        public bool Save<T>(T data) where T : class
        {
            string model = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(SavedData<T>(), model);
            return true;
        }
    }
}