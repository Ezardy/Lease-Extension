using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using Zenject;

namespace LeaseExtension.World.Track
{
    [UsedImplicitly]
    public class TrackFactory : PlaceholderFactory<float, float, byte, int, ITrack>
    {
        public override ITrack Create(float y, float scale, byte maxConstructions, int order)
        {
            return base.Create(y, scale, maxConstructions, order);
        }
    }
}
