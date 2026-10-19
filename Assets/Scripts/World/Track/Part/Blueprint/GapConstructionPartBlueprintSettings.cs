using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "GapConstructionPartBlueprint",
        menuName = "Scriptable Objects/Blueprints/Gap Construction Part Blueprint")]
    internal class GapConstructionPartBlueprintSettings : ScriptableObject, IConstructionPartBlueprintSettings
    {
        [SerializeField, Range(0, 1)] private float _gapSize;
        [SerializeField] private Ref<IConstructionPartBlueprintSettings> _bottomPart;
        [SerializeField] private Ref<IConstructionPartBlueprintSettings> _gapPart;
        [SerializeField] private Ref<IConstructionPartBlueprintSettings> _topPart;

        public IReadOnlyCollection<IConstructionPartBlueprintSettings> SubPartSettings =>
            new[] { _bottomPart.I, _gapPart.I, _topPart.I };

        public float GapSize => _gapSize;
    }
}