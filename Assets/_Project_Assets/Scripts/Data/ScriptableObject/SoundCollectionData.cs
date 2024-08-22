using UnityEngine;

namespace Scripts.Data.ScriptableObject
{
    [CreateAssetMenu(fileName = "SoundCollectionData", menuName = "Data/SoundCollectionData", order = 0)]
    public class SoundCollectionData : UnityEngine.ScriptableObject
    {
        [field: SerializeField] public AudioClip CardClip { get; private set; }
        [field: SerializeField] public AudioClip Match { get; private set; }
        [field: SerializeField] public AudioClip MisMatch { get; private set; }
        [field: SerializeField] public AudioClip GameEnd { get; private set; }
    }
}