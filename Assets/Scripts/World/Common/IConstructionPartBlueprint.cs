using System.Collections.Generic;

namespace Aniki.World {
	public interface IConstructionPartBlueprint {
		public IReadOnlyCollection<IConstructionPartBlueprint>	SubPartBlueprints { get; }

		public float	Width { get; }
		public float	InterfereWidth { get; }
		public float	Margin { get; }

		public IConstructionBlank	MakeBlank(float height, float size);
	}

	public interface IGapConstructionPartBlueprint : IConstructionPartBlueprint {
		public float	GapSize { get; }
	}
}
