namespace LeaseExtension.Cosmetics.Contract
{
    public interface ICosmeticsItemModel
    {
        public string Id { get; }
        public uint Price { get; }

        public ICosmeticsSpriteHandles GetSpriteHandles();
    }
}
