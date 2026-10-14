using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class VirtualConstructionPartBlueprint : IConstructionPartBlueprint
    {
        private readonly VirtualConstructionPartBlueprintSettings _settings;
        private readonly VirtualConstructionPart.Factory _partFactory;
        private readonly VirtualConstructionBlank.Factory _blankFactory;

        public float Width => 0;
        public float InterfereWidth => _settings.InterfereWidth;
        public float Margin => 0;

        public VirtualConstructionPartBlueprint(VirtualConstructionPartBlueprintSettings settings,
            VirtualConstructionPart.Factory partFactory, VirtualConstructionBlank.Factory blankFactory)
        {
            _settings = settings;
            _partFactory = partFactory;
            _blankFactory = blankFactory;
        }

        public IConstructionBlank MakeBlank(float height, float size)
        {
            return _blankFactory.Create(this, _partFactory);
        }
    }
}