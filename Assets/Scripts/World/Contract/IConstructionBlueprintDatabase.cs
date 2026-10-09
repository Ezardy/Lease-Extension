using System.Collections.Generic;

namespace LeaseExtension.World.Contract
{
    public interface IConstructionBlueprintDatabase
    {
        public IReadOnlyCollection<IConstructionBlueprint> Constructions { get; }
    }
}
