using System.Collections.Generic;
using UnityEngine;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "CollectableConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Soaring Construction Blueprint")]
	internal class SoaringConstructionBlueprint : ConstructionBlueprint {
		[SerializeField, Range(0, 1)] private float	startHeight = 0;
		[SerializeField, Range(0, 1)] private float	endHeight = 1;

		public override IReadOnlyCollection<IConstructionBlank>	MakeBlanks() {
			blanks.Clear();
			foreach (IConstructionPartBlueprint part in this)
				blanks.Add(part.MakeBlank(Random.Range(startHeight, endHeight), height));
			return blanks;
		}
	}
}
