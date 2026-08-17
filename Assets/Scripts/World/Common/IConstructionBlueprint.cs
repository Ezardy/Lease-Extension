using System.Collections.Generic;

namespace Aniki.World {
	public interface IConstructionBlueprint {
		public IReadOnlyCollection<IConstructionPartBlueprint>	Parts { get; }

		public float	MinMargin { get; }
		public float	MaxMargin { get; }
		public float	MinDelay { get; }
		public float	MaxDelay { get; }
		public float	RangeStart { get; }
		public float	RangeEnd { get; }
		public bool		IsRangeInversed { get; }

		public IReadOnlyCollection<IConstructionBlank>	MakeBlanks();
	}
}
