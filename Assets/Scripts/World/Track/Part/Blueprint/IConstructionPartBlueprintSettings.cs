using System.Collections.Generic;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    internal interface IConstructionPartBlueprintSettings
    {
        public IReadOnlyCollection<IConstructionPartBlueprintSettings> SubPartSettings { get; }
    }
}
