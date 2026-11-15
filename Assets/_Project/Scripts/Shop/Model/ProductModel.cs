using LeaseExtension.Common.Utilities;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using Unity.Properties;
using UnityEngine;

namespace LeaseExtension.Shop.Model
{
    [CreateAssetMenu(fileName = "CosmeticsVariant", menuName = "Scriptable Objects/Cosmetics Variant")]
    internal class ProductModel : SerializableScriptableObject, ICosmeticsModel, IProductModel
    {
        [SerializeField] private string _label;
        [SerializeField] private uint _price;
        [SerializeField] private Sprite _back;
        [SerializeField] private Sprite _front;

        [CreateProperty] public string Label => _label;
        [CreateProperty] public uint Price => _price;
        [CreateProperty] public Texture2D BackPreview => _back ? _back.texture : null;
        [CreateProperty] public Texture2D FrontPreview => _front ? _front.texture : null;
        public Sprite Back => _back;
        public Sprite Front => _front;
    }
}