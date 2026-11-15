using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class GapConstructionPartBlueprintFactory :
        ConstructionPartBlueprintFactoryBase<GapConstructionPartBlueprintSettings, GapConstructionPartBlueprint>
    {
        public GapConstructionPartBlueprintFactory(DiContainer container) : base(container)
        {}

        public override GapConstructionPartBlueprint Create(GapConstructionPartBlueprintSettings settings,
            IReadOnlyList<IConstructionPartBlueprint> subParts)
        {
            return Container.Instantiate<GapConstructionPartBlueprint>(
                new object[] { settings, subParts[0], subParts[1], subParts[2] });
        }
    }
}