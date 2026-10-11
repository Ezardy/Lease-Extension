using System.Collections.Generic;
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

        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints { get; }
        public float Width { get; } // TODO: do not store all setting's properties in the instance. Just reference them
        public float InterfereWidth { get; }
        public float Margin { get; }

        public ConstructionPartBlueprint(ConstructionPartBlueprintSettings settings,
            ConstructionPart.Factory partFactory, IReadOnlyList<IConstructionPartBlueprint> subParts,
            ConstructionBlank.Factory blankFactory)
        {
            Width = settings.Width;
            InterfereWidth = settings.InterfereWidth;
            Margin = settings.Margin;
            _partFactory = partFactory;
            _blankFactory = blankFactory;
            SubPartBlueprints = subParts;
        }

        public IConstructionBlank MakeBlank(float height, float size)
        {
            return _blankFactory.Create(this, _partFactory, height, size);
        }
    }
}
