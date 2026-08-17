using R3;

namespace Aniki.World {
	public interface IWorldModel {
		public float	Speed { get; }
		public float	Perspective { get; }
		public float	Gravity { get; }

		public float	SpeedAmplifier { get; set; }

		public Observable<float>	SpeedChanged { get; }
		public Observable<float>	PerspectiveChanged { get; }
		public Observable<float>	GravityChanged { get; }
	}
}
