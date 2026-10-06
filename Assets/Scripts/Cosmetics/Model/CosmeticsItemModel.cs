using LeaseExtension.Cosmetics.Contract;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LeaseExtension.Cosmetics.Model
{
    [CreateAssetMenu(fileName = "CosmeticsVariant", menuName = "Scriptable Objects/Cosmetics Variant")]
    internal class CosmeticsItemModel : ScriptableObject, ICosmeticsItemModel
    {
        [SerializeField] private string _id;
        [SerializeField] private uint _price;
        [SerializeField] private AssetReferenceSprite _backSpriteAssetReference;
        [SerializeField] private AssetReferenceSprite _frontSpriteAssetReference;

        public string Id => _id;
        public uint Price => _price;

        public ICosmeticsSpriteHandles GetSpriteHandles()
        {
            return new CosmeticsSpriteHandles(
                _backSpriteAssetReference.RuntimeKeyIsValid() ? _backSpriteAssetReference.LoadAssetAsync() : new(),
                _frontSpriteAssetReference.RuntimeKeyIsValid() ? _frontSpriteAssetReference.LoadAssetAsync() : new());
        }
    }
}
