using LeaseExtension.World.Contract;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.World.Track
{
    [MovedFrom("LeaseExtension")]
    public class ITrackFactory : PlaceholderFactory<float, float, byte, int, ITrack>
    {
        public override ITrack Create(float y, float scale, byte maxConstructions, int order)
        {
            return base.Create(y, scale, maxConstructions, order);
        }
    }
}
