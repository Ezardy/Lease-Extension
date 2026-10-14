namespace LeaseExtension.World.Contract
{
    public interface IConstructionPartBlueprint
    {
        public float Width { get; }
        public float InterfereWidth { get; }
        public float Margin { get; }

        public IConstructionBlank MakeBlank(float height, float size);
    }
}