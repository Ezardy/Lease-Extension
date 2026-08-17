using Aniki.Cosmetics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CosmeticsDatabase", menuName = "Scriptable Objects/Cosmetics Database")]
internal class CosmeticsItemModelDatabase : ScriptableObject, ICosmeticsItemModelDatabase {
	[SerializeField] private List<CosmeticsItemModel>	variants;

	private Dictionary<string, CosmeticsItemModel>	variantsDict;

	public ICosmeticsItemModel	GetCosmeticsItemModel(string id) {
		return variantsDict[id];
	}

	public IEnumerable<ICosmeticsItemModel>	GetCosmeticsItemModels() {
		return variants;
	}

	private void	OnEnable() {
		if (variants != null)
			variantsDict = variants.ToDictionary(p => p.Id);
	}
}
