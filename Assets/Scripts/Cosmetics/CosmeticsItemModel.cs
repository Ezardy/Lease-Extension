using LeaseExtension.Cosmetics.Contract;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace LeaseExtension.Cosmetics
{
    [CreateAssetMenu(fileName = "CosmeticsVariant", menuName = "Scriptable Objects/Cosmetics Variant")]
    [MovedFrom("Aniki.Cosmetics")]
    internal class CosmeticsItemModel : ScriptableObject, ICosmeticsItemModel
    {
        [SerializeField]
        [FormerlySerializedAs("id")]
        private string _id;

        [SerializeField]
        [FormerlySerializedAs("price")]
        private uint _price;

        [SerializeField]
        [FormerlySerializedAs("backSpriteAssetReference")]
        private AssetReferenceSprite _backSpriteAssetReference;

        [SerializeField]
        [FormerlySerializedAs("frontSpriteAssetReference")]
        private AssetReferenceSprite _frontSpriteAssetReference;

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
