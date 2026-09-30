using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace LeaseExtension.Common.Layer
{
    [CreateAssetMenu(fileName = "LayerNames", menuName = "Scriptable Objects/Layer Names")]
    [MovedFrom("Aniki.Common")]
    public class LayerNames : ScriptableObject
    {
        [SerializeField]
        [FormerlySerializedAs("defaultLayerName")]
        private string _defaultLayerName;

        [SerializeField]
        [FormerlySerializedAs("transparentFXLayerName")]
        private string _transparentFXLayerName;

        [SerializeField]
        [FormerlySerializedAs("ignoreRaycastLayerName")]
        private string _ignoreRaycastLayerName;

        [SerializeField]
        [FormerlySerializedAs("waterLayerName")]
        private string _waterLayerName;

        [SerializeField]
        [FormerlySerializedAs("uILayerName")]
        private string _uILayerName;

        [SerializeField]
        [FormerlySerializedAs("playerLayerName")]
        private string _playerLayerName;

        public string Default => _defaultLayerName;
        public string TransparentFX => _transparentFXLayerName;
        public string IgnoreRaycast => _ignoreRaycastLayerName;
        public string Water => _waterLayerName;
        public string UI => _uILayerName;
        public string Player => _playerLayerName;
    }
}
