namespace LeaseExtension.World.Contract
{
    public interface IGapConstructionPartBlueprint : IConstructionPartBlueprint
    {
        public float GapSize { get; }

        public IConstructionBlank MakeBlank(float height, float size, float gapPosition);
    }
}