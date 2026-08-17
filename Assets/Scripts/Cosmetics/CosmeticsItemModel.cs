using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Aniki.Cosmetics {
	[CreateAssetMenu(fileName = "CosmeticsVariant", menuName = "Scriptable Objects/Cosmetics Variant")]
	public class CosmeticsItemModel : ScriptableObject, ICosmeticsItemModel {
		[SerializeField] private string					id;
		[SerializeField] private uint					price;
		[SerializeField] private AssetReferenceSprite	backSpriteAssetReference;
		[SerializeField] private AssetReferenceSprite	frontSpriteAssetReference;

		public string	Id => id;
		public uint		Price => price;

		public ICosmeticsSpriteHandles	GetSpriteHandles() {
			return new CosmeticsSpriteHandles(
				backSpriteAssetReference.RuntimeKeyIsValid() ? backSpriteAssetReference.LoadAssetAsync() : new(),
				frontSpriteAssetReference.RuntimeKeyIsValid() ? frontSpriteAssetReference.LoadAssetAsync() : new()
			);
		}
	}
}
