using LeaseExtension.Common.Contract;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aniki.Common {
	public abstract class SerializableScriptableObject : ScriptableObject, IIdentifiable {
		[SerializeField, HideInInspector] private string	guid;
		public string	GUID => guid;
	
	#if UNITY_EDITOR
		private void	OnValidate()
        {
			guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
		}
	#endif
	}
}
