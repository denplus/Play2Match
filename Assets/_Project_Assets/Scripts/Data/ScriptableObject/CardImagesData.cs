using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data.ScriptableObject
{
    [CreateAssetMenu(fileName = "CardImagesData", menuName = "Data/CardImagesData", order = 0)]
    public class CardImagesData : UnityEngine.ScriptableObject
    {
        [field: SerializeField] public List<Sprite> CardImages { get; private set; }

    }
}