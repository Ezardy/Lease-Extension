using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "GapConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Gap Construction Part Blueprint")]
    internal class GapConstructionPartBlueprintSettings : ScriptableObject, IConstructionPartBlueprintSettings
    {
        [SerializeField, Range(0, 1)]
        private float _gapSize;
        [SerializeField]
        private Ref<IConstructionPartBlueprintSettings> _bottomPart;
        [SerializeField]
        private Ref<IConstructionPartBlueprintSettings> _gapPart;
        [SerializeField]
        private Ref<IConstructionPartBlueprintSettings> _topPart;

        public IConstructionPartBlueprintSettings BottomPart => _bottomPart.I;
        public IConstructionPartBlueprintSettings GapPart => _gapPart.I;
        public IConstructionPartBlueprintSettings TopPart => _topPart.I;
        public IReadOnlyCollection<IConstructionPartBlueprintSettings> SubPartSettings =>
            new[] { BottomPart, GapPart, TopPart };
        public float Width => Mathf.Max(BottomPart.Width, GapPart.Width, TopPart.Width);
        public float Margin => Mathf.Max(BottomPart.Margin, GapPart.Margin, TopPart.Margin);
        public float GapSize => _gapSize;
        public float InterfereWidth => Mathf.Max(BottomPart.InterfereWidth, GapPart.InterfereWidth, TopPart.InterfereWidth);
    }
}
