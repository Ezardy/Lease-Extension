using Aniki.Inspector;
using LeaseExtension.Common.Contract;
using UnityEditor;
using UnityEngine;

namespace LeaseExtension.Common.Utilities
{
    public abstract class SerializableScriptableObject : ScriptableObject, IIdentifiable
    {
        [SerializeField, ReadOnly] private string guid;
        public string GUID => guid;

#if UNITY_EDITOR
        private void OnValidate()
        {
            guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
        }
#endif
    }
}