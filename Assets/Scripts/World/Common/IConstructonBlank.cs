using UnityEngine;

namespace Aniki.World {
	public interface IConstructionBlank {
		public IConstructionPart	Construct(Vector2 position, float scale, int order);
	}
}
