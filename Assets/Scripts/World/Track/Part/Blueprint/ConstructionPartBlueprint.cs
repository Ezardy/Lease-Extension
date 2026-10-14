using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class ConstructionPartBlueprint : IConstructionPartBlueprint
    {
        private readonly ConstructionPart.Factory _partFactory;
        private readonly ConstructionBlank.Factory _blankFactory;
        private readonly ConstructionPartBlueprintSettings _settings;

        public float Width => _settings.Width;
        public float InterfereWidth => _settings.InterfereWidth;
        public float Margin => _settings.Margin;

        public ConstructionPartBlueprint(ConstructionPartBlueprintSettings settings,
            ConstructionPart.Factory partFactory, ConstructionBlank.Factory blankFactory)
        {
            _settings = settings;
            _partFactory = partFactory;
            _blankFactory = blankFactory;
        }

        public IConstructionBlank MakeBlank(float height, float size)
        {
            return _blankFactory.Create(this, _partFactory, height, size);
        }
    }
}