using System.Collections.Generic;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "VirtualConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Virtual Construction Part Blueprint")]
    internal class VirtualConstructionPartBlueprintSettings : ScriptableObject, IConstructionPartBlueprintSettings
    {
        [SerializeField] private byte _initialPool = 5;
        [SerializeField] private float _width = 1;

        public byte InitialPool => _initialPool;
        public IReadOnlyCollection<IConstructionPartBlueprintSettings> SubPartSettings =>
            System.Array.Empty<IConstructionPartBlueprintSettings>();
        public float InterfereWidth => _width;
        public float Width => 0;
        public float Margin => 0;
    }
}
