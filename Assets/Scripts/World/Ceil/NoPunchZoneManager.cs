using LeaseExtension.World.Contract;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.World.Ceil
{
    [MovedFrom("Aniki.World")]
    internal class NoPunchZoneManager : INoPunchZone
    {
        public bool InZone => NoPunchZone.InZone;
    }
}
