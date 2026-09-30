using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.World.Ceil
{
    [MovedFrom("Aniki.World")]
    internal class NoPunchZone : MonoBehaviour
    {
        private static int _zoneCount = 0;

        public static bool InZone => _zoneCount > 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _zoneCount += 1;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _zoneCount -= 1;
        }
    }
}
