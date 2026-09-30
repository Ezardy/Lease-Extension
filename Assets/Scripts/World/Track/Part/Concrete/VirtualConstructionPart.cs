using Aniki.World;

namespace LeaseExtension.World.Track.Part.Concrete
{
    internal class VirtualConstructionPart : IConstructionPart {
        private float	x = 0;

        public VirtualConstructionPart(IConstructionPartBlueprint blueprint) {
            Blueprint = blueprint;
        }

        public IConstructionPartBlueprint	Blueprint { get; }

        public float	X => x;

        public void	Move(float shift) {
            x += shift;
        }

        public void	WipeOut() { }
    }
}
