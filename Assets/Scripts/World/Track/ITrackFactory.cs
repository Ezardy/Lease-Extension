using Aniki.World;
using Zenject;

namespace LeaseExtension
{
    public class ITrackFactory : PlaceholderFactory<float, float, byte, int, ITrack> {
        public override ITrack	Create(float y, float scale, byte maxConstructions, int order) {
            return base.Create(y, scale, maxConstructions, order);
        }
    }
}
