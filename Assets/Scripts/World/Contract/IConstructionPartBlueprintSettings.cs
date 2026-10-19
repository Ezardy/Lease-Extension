using System.Collections.Generic;

namespace LeaseExtension.World.Contract
{
    public interface IConstructionPartBlueprintSettings
    {
        public IReadOnlyCollection<IConstructionPartBlueprintSettings> SubPartSettings { get; }
    }
}