using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class VirtualConstructionPartBlueprint : IConstructionPartBlueprint
    {
        private readonly VirtualConstructionPart.Factory _partFactory;
        private readonly VirtualConstructionBlank.Factory _blankFactory;

        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints { get; }
        public float Width => 0;
        public float InterfereWidth { get; } // TODO: use setting's value
        public float Margin => 0;

        public VirtualConstructionPartBlueprint(VirtualConstructionPartBlueprintSettings settings,
            VirtualConstructionPart.Factory partFactory, IReadOnlyList<IConstructionPartBlueprint> subParts,
            VirtualConstructionBlank.Factory blankFactory)
        {
            InterfereWidth = settings.InterfereWidth;
            _partFactory = partFactory;
            _blankFactory = blankFactory;
            SubPartBlueprints = subParts;
        }

        public IConstructionBlank MakeBlank(float height, float size)
        {
            return _blankFactory.Create(this, _partFactory);
        }
    }
}
