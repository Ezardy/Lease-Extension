using System.Collections.Generic;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "ConstructionPartBlueprint",
        menuName = "Scriptable Objects/Blueprints/Construction Part Blueprint")]
    internal class ConstructionPartBlueprintSettings : ScriptableObject, IConstructionPartBlueprintSettings
    {
        [SerializeField]
        protected GameObject Prefab;
        [SerializeField]
        private byte _initialPool = 5;
        [SerializeField, Space]
        private float _margin;
        [SerializeField]
        private float _width;
        [SerializeField]
        private bool _autoWidth = true;
        [SerializeField, Space]
        private float _interfereWidth;
        [SerializeField]
        private bool _autoInterfereWidth = true;
        [SerializeField]
        private bool _sameWidth = true;
        [SerializeField, Space]
        private bool _useSpriteRenderer = true;
        [SerializeField]
        private bool _zOrdering;

        public GameObject PartPrefab => Prefab;
        public byte InitialPool => _initialPool;
        public bool UseSpriteRenderer => _useSpriteRenderer;
        public bool ZOrdering => _zOrdering;

        public IReadOnlyCollection<IConstructionPartBlueprintSettings> SubPartSettings =>
            System.Array.Empty<IConstructionPartBlueprintSettings>();

        public float Width => (_autoWidth ? AutoWidth : _width) + _margin;

        public float InterfereWidth => _sameWidth
            ? Width
            : (_autoInterfereWidth ? AutoWidth : _interfereWidth) + _margin;

        public float Margin => _margin;

        private float AutoWidth => _useSpriteRenderer
            ? Prefab.GetComponent<SpriteRenderer>().bounds.size.x
            : Prefab.GetComponent<BoxCollider2D>().bounds.size.x;
    }
}