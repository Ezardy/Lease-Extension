using JetBrains.Annotations;
using LeaseExtension.World.Contract;

namespace LeaseExtension.World.Ceil
{
    [UsedImplicitly]
    internal class NoPunchZoneManager : INoPunchZone
    {
        public bool InZone => NoPunchZone.InZone;
    }
}
