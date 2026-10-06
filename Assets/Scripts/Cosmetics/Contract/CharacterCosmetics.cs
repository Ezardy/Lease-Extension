using Zenject;

namespace LeaseExtension.Cosmetics.Contract
{
    public class CharacterCosmetics : Kernel
    {
        public ICosmeticsElementModel Model { get; }
        public ICosmeticsItemModelDatabase Database { get; }

        public CharacterCosmetics(ICosmeticsElementModel model, ICosmeticsItemModelDatabase database)
        {
            Model = model;
            Database = database;
        }
    }
}
