using UnityEngine;

namespace Aniki.World {
	internal class NoPunchZone : MonoBehaviour {
		private static int	zoneCount = 0;

		public static bool	InZone => zoneCount > 0;

		private void	OnTriggerEnter2D(Collider2D collision) {
			zoneCount += 1;
		}

		private void	OnTriggerExit2D(Collider2D collision) {
			zoneCount -= 1;
		}
	}
}
