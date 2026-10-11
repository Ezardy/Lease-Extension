using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class GapConstructionPartBlueprint : IGapConstructionPartBlueprint
    {
        private readonly IConstructionPartBlueprint _bottomPart;
        private readonly IConstructionPartBlueprint _gapPart;
        private readonly IConstructionPartBlueprint _topPart;
        private readonly GapConstructionBlank.Factory _blankFactory;

        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints { get; }
        public float Width { get; } // TODO: use setting's value
        public float InterfereWidth { get; }
        public float Margin { get; }
        public float GapSize { get; }

        public GapConstructionPartBlueprint(GapConstructionPartBlueprintSettings settings,
            IConstructionPartBlueprint bottomPart, IConstructionPartBlueprint gapPart,
            IConstructionPartBlueprint topPart, GapConstructionBlank.Factory blankFactory)
        {
            _bottomPart = bottomPart;
            _gapPart = gapPart;
            _topPart = topPart;
            _blankFactory = blankFactory;
            GapSize = settings.GapSize;
            SubPartBlueprints = new[] { bottomPart, gapPart, topPart };
            Width = Mathf.Max(bottomPart.Width, gapPart.Width, topPart.Width);
            InterfereWidth = Mathf.Max(bottomPart.InterfereWidth, gapPart.InterfereWidth, topPart.InterfereWidth);
            Margin = Mathf.Max(bottomPart.Margin, gapPart.Margin, topPart.Margin);
        }

        public IConstructionBlank MakeBlank(float height, float size)
        {
            return MakeBlank(height, size, 0.5f);
        }

        public IConstructionBlank MakeBlank(float height, float size, float gapPosition)
        {
            float gapHeight = size * GapSize;
            float gapStart = height + size * (Mathf.Clamp(gapPosition, GapSize / 2, 1 - GapSize / 2) - GapSize / 2);
            float gapEnd = gapStart + gapHeight;
            return _blankFactory.Create(this, _bottomPart.MakeBlank(height, gapStart - height),
                _gapPart.MakeBlank(gapStart, gapHeight), _topPart.MakeBlank(gapEnd, height + size - gapEnd));
        }
    }
}
