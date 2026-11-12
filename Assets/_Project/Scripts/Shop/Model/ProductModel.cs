using LeaseExtension.Common.Utilities;
using LeaseExtension.Shop.Contract;
using Unity.Properties;
using UnityEngine;

namespace LeaseExtension.Shop.Model
{
    [CreateAssetMenu(fileName = "CosmeticsVariant", menuName = "Scriptable Objects/Cosmetics Variant")]
    internal class ProductModel : SerializableScriptableObject, IProductModel
    {
        [SerializeField] private string _label;
        [SerializeField] private uint _price;
        [SerializeField] private Sprite _back;
        [SerializeField] private Sprite _front;

        [CreateProperty] public string Label => _label;
        [CreateProperty] public uint Price => _price;
        [CreateProperty] public Sprite Back => _back;
        [CreateProperty] public Sprite Front => _front;
    }
}
