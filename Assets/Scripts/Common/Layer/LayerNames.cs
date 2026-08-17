using UnityEngine;

namespace Aniki.Common {
	[CreateAssetMenu(fileName = "LayerNames", menuName = "Scriptable Objects/Layer Names")]
	public class LayerNames : ScriptableObject {
		[SerializeField] private string	defaultLayerName;
		[SerializeField] private string	transparentFXLayerName;
		[SerializeField] private string	ignoreRaycastLayerName;
		[SerializeField] private string	waterLayerName;
		[SerializeField] private string	uILayerName;
		[SerializeField] private string	playerLayerName;

		public string	Default => defaultLayerName;
		public string	TransparentFX => transparentFXLayerName;
		public string	IgnoreRaycast => ignoreRaycastLayerName;
		public string	Water => waterLayerName;
		public string	UI => uILayerName;
		public string	Player => playerLayerName;
	}
}
