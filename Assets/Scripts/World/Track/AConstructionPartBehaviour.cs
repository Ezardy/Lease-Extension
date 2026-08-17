using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal abstract class AConstructionPartBehaviour : MonoBehaviour {
		public class Factory : PlaceholderFactory<Object, AConstructionPartBehaviour> { }
	}
}
