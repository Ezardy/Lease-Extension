using UnityEngine;

namespace LeaseExtension.Common.Layer
{
    [CreateAssetMenu(fileName = "LayerNames", menuName = "Scriptable Objects/Layer Names")]
    public class LayerNames : ScriptableObject
    {
        [SerializeField]
        private string _defaultLayerName;

        [SerializeField]
        private string _transparentFXLayerName;

        [SerializeField]
        private string _ignoreRaycastLayerName;

        [SerializeField]
        private string _waterLayerName;

        [SerializeField]
        private string _uILayerName;

        [SerializeField]
        private string _playerLayerName;

        public string Default => _defaultLayerName;
        public string TransparentFX => _transparentFXLayerName;
        public string IgnoreRaycast => _ignoreRaycastLayerName;
        public string Water => _waterLayerName;
        public string UI => _uILayerName;
        public string Player => _playerLayerName;
    }
}
