namespace Aniki.Cosmetics {
	public interface ICosmeticsItemModel {
		public string	Id { get; }
		public uint		Price { get; }

		public ICosmeticsSpriteHandles	GetSpriteHandles();
	}
}
