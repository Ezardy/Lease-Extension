namespace Aniki.World {
	public interface IConstructionPart {
		public IConstructionPartBlueprint	Blueprint { get; }
		public float						X { get; }

		public void	Move(float shift);
		public void	WipeOut();
	}
}
