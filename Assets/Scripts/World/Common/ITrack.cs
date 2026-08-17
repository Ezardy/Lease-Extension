using Zenject;

namespace Aniki.World {
	public interface ITrack {
		public float	StartX { get; set; }
		public float	EndX { get; set; }
		public float	Speed { get; set; }

		public void	Construct(IConstructionBlank blank);
		public void	Move();
		public void	WipeOut();

		public bool	IsFree { get; }
	}

	public class ITrackFactory : PlaceholderFactory<float, float, byte, int, ITrack> {
		public override ITrack	Create(float y, float scale, byte maxConstructions, int order) {
			return base.Create(y, scale, maxConstructions, order);
		}
	}
}
