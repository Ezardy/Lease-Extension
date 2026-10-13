using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class GapConstructionPartBlueprint : IGapConstructionPartBlueprint
    {
        private readonly GapConstructionPartBlueprintSettings _settings;
        private readonly IConstructionPartBlueprint _bottomPart;
        private readonly IConstructionPartBlueprint _gapPart;
        private readonly IConstructionPartBlueprint _topPart;
        private readonly GapConstructionBlank.Factory _blankFactory;

        public float Width => Mathf.Max(_bottomPart.Width, _gapPart.Width, _topPart.Width);

        public float InterfereWidth =>
            Mathf.Max(_bottomPart.InterfereWidth, _gapPart.InterfereWidth, _topPart.InterfereWidth);

        public float Margin => Mathf.Max(_bottomPart.Margin, _gapPart.Margin, _topPart.Margin);
        public float GapSize => _settings.GapSize;

        public GapConstructionPartBlueprint(GapConstructionPartBlueprintSettings settings,
            IConstructionPartBlueprint bottomPart, IConstructionPartBlueprint gapPart,
            IConstructionPartBlueprint topPart, GapConstructionBlank.Factory blankFactory)
        {
            _settings = settings;
            _bottomPart = bottomPart;
            _gapPart = gapPart;
            _topPart = topPart;
            _blankFactory = blankFactory;
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