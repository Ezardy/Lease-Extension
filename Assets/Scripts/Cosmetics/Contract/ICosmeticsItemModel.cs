namespace Aniki.Cosmetics {
	internal interface ICosmeticsItemModel {
		public string	Id { get; }
		public uint		Price { get; }

		public ICosmeticsSpriteHandles	GetSpriteHandles();
	}
}
