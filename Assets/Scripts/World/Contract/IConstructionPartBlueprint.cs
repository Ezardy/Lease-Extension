using System.Collections.Generic;

namespace LeaseExtension.World.Contract
{
    public interface IConstructionPartBlueprint
    {
        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints { get; }
        public float Width { get; }
        public float InterfereWidth { get; }
        public float Margin { get; }

        public IConstructionBlank MakeBlank(float height, float size);
    }
}
