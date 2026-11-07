using Aniki.Common;
using LeaseExtension.Shop.Contract;
using UnityEngine;

namespace LeaseExtension.Shop.Model
{
    [CreateAssetMenu(fileName = "CosmeticsVariant", menuName = "Scriptable Objects/Cosmetics Variant")]
    internal class ProductModel : SerializableScriptableObject, IProductModel
    {
        [SerializeField] private uint _price;
        [SerializeField] private Sprite _back;
        [SerializeField] private Sprite _front;

        public uint Price => _price;
        public Sprite Back => _back;
        public Sprite Front => _front;
    }
}
