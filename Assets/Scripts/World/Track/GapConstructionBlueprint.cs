using System.Collections.Generic;
using UnityEngine;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "GapConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Gap Construction Blueprint")]
	internal class GapConstructionBlueprint : ConstructionBlueprint {
		public override IReadOnlyCollection<IConstructionBlank>	MakeBlanks() {
			float	size = 0;
			float	gapPos;

			blanks.Clear();
			foreach (IConstructionPartBlueprint part in this) {
				if (part is IGapConstructionPartBlueprint gap)
					size = Mathf.Max(size, gap.GapSize);
			}
			gapPos = Random.Range(size / 2, 1 - size / 2);
			foreach (IConstructionPartBlueprint part in this) {
				if (part is IGapConstructionPartBlueprint gap)
					blanks.Add(gap.MakeBlank(0, gapPos));
				else
					blanks.Add(part.MakeBlank(0, 1));
			}
			return blanks;
		}
	}
}
