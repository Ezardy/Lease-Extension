using System.Collections.Generic;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    internal interface IConstructionPartBlueprintSettings
    {
        public IReadOnlyCollection<IConstructionPartBlueprintSettings> SubPartSettings { get; }
        public float Width { get; }
        public float InterfereWidth { get; }
        public float Margin { get; }
    }
}
