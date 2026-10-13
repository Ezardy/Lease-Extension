using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class VirtualConstructionPartBlueprintFactory :
        AConstructionPartBlueprintFactory<VirtualConstructionPartBlueprintSettings, VirtualConstructionPartBlueprint>
    {
        public VirtualConstructionPartBlueprintFactory(DiContainer container) : base(container)
        {
        }

        public override VirtualConstructionPartBlueprint Create(VirtualConstructionPartBlueprintSettings settings,
            IReadOnlyList<IConstructionPartBlueprint> subParts)
        {
            return Container.Instantiate<VirtualConstructionPartBlueprint>(
                new object[] { settings, Container.ResolveId<VirtualConstructionPart.Factory>(settings) });
        }

        protected override void InstallPools(VirtualConstructionPartBlueprintSettings settings)
        {
            Container
                .BindFactory<IConstructionPartBlueprint, VirtualConstructionPart, VirtualConstructionPart.Factory>()
                .WithId(settings).FromPoolableMemoryPool(pool => pool.WithInitialSize(settings.InitialPool));
        }
    }
}