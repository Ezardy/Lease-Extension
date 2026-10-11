using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;

namespace LeaseExtension.World.Track.Construction
{
    [UsedImplicitly]
    internal class ConstructionBlueprintDatabase : IConstructionBlueprintDatabase
    {
        public IReadOnlyCollection<IConstructionBlueprint> ConstructionBlueprints { get; }

        public ConstructionBlueprintDatabase(ConstructionBlueprintDatabaseSettings settings, ConstructionBlueprint.Factory factory)
        {
            List<IConstructionBlueprint> constructions = new();
            foreach (ConstructionBlueprintSettings setting in settings.Constructions)
            {
                constructions.Add(factory.Create(setting));
            }
            ConstructionBlueprints = constructions.AsReadOnly();
        }
    }
}
