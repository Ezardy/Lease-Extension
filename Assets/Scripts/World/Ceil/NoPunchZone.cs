using UnityEngine;

namespace LeaseExtension.World.Ceil
{
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
