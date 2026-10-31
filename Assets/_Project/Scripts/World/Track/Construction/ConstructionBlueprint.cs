using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blueprint;
using Zenject;

namespace LeaseExtension.World.Track.Construction
{
    [UsedImplicitly]
    internal class ConstructionBlueprint : IConstructionBlueprint
    {
        private readonly ConstructionBlueprintSettings _settings;

        public IReadOnlyCollection<IConstructionPartBlueprint> Parts { get; }
        public float MinDelay => _settings.MinDelay;
        public float MaxDelay => _settings.MaxDelay;
        public float MinMargin => _settings.MinMargin;
        public float MaxMargin => _settings.MaxMargin;
        public float RangeStart => _settings.RangeStart;
        public float RangeEnd => _settings.RangeEnd;
        public bool IsRangeInversed => _settings.IsRangeInversed;

        public ConstructionBlueprint(ConstructionBlueprintSettings settings,
            ConstructionPartBlueprintFactory.Factory partBlueprintFactory)
        {
            _settings = settings;
            List<IConstructionPartBlueprint> parts = new();
            foreach (IConstructionPartBlueprintSettings part in settings.PartSettings)
            {
                parts.Add(partBlueprintFactory.Create(part));
            }

            Parts = parts.AsReadOnly();
        }

        public void MakeBlanks(ICollection<IConstructionBlank> blanksOutput)
        {
            _settings.MakeBlanks(Parts, blanksOutput);
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<ConstructionBlueprintSettings, ConstructionBlueprint>
        {}
    }
}