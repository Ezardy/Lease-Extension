using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [UsedImplicitly]
    internal class PrefabConstructionPartBlueprintFactory :
        AConstructionPartBlueprintFactory<ConstructionPartBlueprintSettings, ConstructionPartBlueprint>
    {
        public PrefabConstructionPartBlueprintFactory(DiContainer container) : base(container)
        {
        }

        public override void InstallPools(ConstructionPartBlueprintSettings settings)
        {
            Container.BindFactory<IConstructionPartBlueprint, ConstructionPart, ConstructionPart.Factory>()
                .WithId(settings).FromPoolableMemoryPool(pool => pool.WithInitialSize(settings.InitialPool)
                    .FromIFactory(factory => factory.To<ConstructionPartFactory>().AsCached().WithArguments(settings)));
        }

        public override ConstructionPartBlueprint Create(ConstructionPartBlueprintSettings settings,
            IReadOnlyList<IConstructionPartBlueprint> subParts)
        {
            return Container.Instantiate<ConstructionPartBlueprint>(
                new object[] { settings, Container.ResolveId<ConstructionPart.Factory>(settings) });
        }
    }
}
