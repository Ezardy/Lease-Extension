using System;
using System.Collections.Generic;
using LeaseExtension.World.Contract;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    internal interface IConstructionPartBlueprintFactory
    {
        public Type SettingsType { get; }

        public void InstallPools(IConstructionPartBlueprintSettings settings);

        public IConstructionPartBlueprint Create(IConstructionPartBlueprintSettings settings,
            IReadOnlyList<IConstructionPartBlueprint> subParts);
    }
}
