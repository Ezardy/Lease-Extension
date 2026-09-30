using LeaseExtension.World.Contract;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class VirtualConstructionPart : IConstructionPart
    {
        public IConstructionPartBlueprint Blueprint { get; }
        public float X { get; private set; } = 0;

        public VirtualConstructionPart(IConstructionPartBlueprint blueprint)
        {
            Blueprint = blueprint;
        }

        public void Move(float shift)
        {
            X += shift;
        }

        public void WipeOut()
        {
        }
    }
}
