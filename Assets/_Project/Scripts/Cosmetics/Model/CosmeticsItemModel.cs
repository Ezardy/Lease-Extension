using Aniki.Common;
using LeaseExtension.Cosmetics.Contract;
using UnityEngine;

namespace LeaseExtension.Cosmetics.Model
{
    [CreateAssetMenu(fileName = "CosmeticsVariant", menuName = "Scriptable Objects/Cosmetics Variant")]
    internal class CosmeticsItemModel : SerializableScriptableObject, ICosmeticsItemModel
    {
        [SerializeField] private uint _price;
        [SerializeField] private Sprite _back;
        [SerializeField] private Sprite _front;

        public uint Price => _price;
        public Sprite Back => _back;
        public Sprite Front => _front;
    }
}
