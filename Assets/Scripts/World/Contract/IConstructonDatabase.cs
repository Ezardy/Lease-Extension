using System.Collections.Generic;

namespace Aniki.World {
	public interface IConstructionBlueprintDatabase {
		public IReadOnlyCollection<IConstructionBlueprint>	Constructions { get; }
	}
}
