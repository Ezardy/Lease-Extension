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
}
