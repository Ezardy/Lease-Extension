using LeaseExtension.World.Contract;

namespace LeaseExtension.World.Ceil
{
    internal class NoPunchZoneManager : INoPunchZone
    {
        public bool InZone => NoPunchZone.InZone;
    }
}
