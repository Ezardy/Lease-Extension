using LeaseExtension.Common.Utilities;
using LeaseExtension.World.Ceil;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Floor;
using LeaseExtension.World.Track;
using LeaseExtension.World.Track.Construction;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Blueprint;
using LeaseExtension.World.Track.Part.Concrete;
using LeaseExtension.World.Wall;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World
{
    internal class WorldMonoInstaller : MonoInstaller
    {
        [SerializeField] private SpriteRenderer _floorSpriteRenderer;
        [SerializeField] private Animator _floorAnimator;
        [SerializeField] private MonoBehaviour _floorViewModel;
        [SerializeField] private SpriteRenderer _wallSpriteRenderer;
        [SerializeField, Space] private byte _trackCount = 11;
        [SerializeField] private byte _maxConstructionCount = 100;
        [SerializeField] private byte _reservedOrderCount = 5;
        [SerializeField, Space] private Ref<IWorldModel> _worldModel;
        [SerializeField] private Ref<ConstructionBlueprintDatabaseSettings> _constructionDatabase;

        public override void InstallBindings()
        {
            InstallWall();
            InstallFloor();
            InstallTracks();
            Container.BindInstance(_worldModel.I);
            Container.QueueForInject(_worldModel.I);
            Container.Bind<INoPunchZone>().To<NoPunchZoneManager>().AsSingle();
            Container.BindInterfacesTo<WorldViewModel>().AsSingle();
            Container.BindFactory<float, float, byte, int, ITrack, TrackFactory>()
                .To<LeaseExtension.World.Track.Track>();
            InstallPartBlueprints();
        }
        private void InstallTracks()
        {
            float depth = _floorSpriteRenderer.bounds.size.y;
            float centerY = _floorSpriteRenderer.transform.position.y;
            Container.BindInterfacesTo<TrackOrchestrator>().AsSingle().WithArguments(
                _trackCount,
                depth,
                centerY,
                _maxConstructionCount,
                _reservedOrderCount);
        }

        private void InstallWall()
        {
            Container.BindInterfacesTo<WallView>().AsSingle().WithArguments(_wallSpriteRenderer);
            Container.BindInterfacesTo<WallViewModel>().AsSingle();
        }

        private void InstallFloor()
        {
            Container.BindInstance(_floorAnimator).WhenInjectedInto<FloorViewModel>();
            Container.BindInterfacesTo<FloorView>().AsSingle().WithArguments(_floorSpriteRenderer);
            Container.QueueForInject(_floorViewModel);
        }

        private void InstallPartBlueprints()
        {
            IConstructionPartBlueprintFactory[] factories =
            {
                new PrefabConstructionPartBlueprintFactory(Container),
                new GapConstructionPartBlueprintFactory(Container),
                new VirtualConstructionPartBlueprintFactory(Container)
            };
            foreach (IConstructionPartBlueprintFactory factory in factories)
            {
                Container.QueueForInject(factory);
            }
            ConstructionPartBlueprintFactory blueprintFactory = new(factories);
            Container.BindInstance(blueprintFactory);
            Container
                .BindFactory<IConstructionPartBlueprint, ConstructionPart.Factory, float, float, ConstructionBlank,
                    ConstructionBlank.Factory>()
                .FromPoolableMemoryPool();
            Container.BindFactory<IConstructionPartBlueprint, IConstructionPart, IConstructionPart, IConstructionPart,
                GapConstructionPart, GapConstructionPart.Factory>().FromPoolableMemoryPool();
            Container
                .BindFactory<IConstructionPartBlueprint, IConstructionBlank, IConstructionBlank, IConstructionBlank,
                    GapConstructionBlank, GapConstructionBlank.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<IConstructionPartBlueprint, VirtualConstructionPart.Factory,
                VirtualConstructionBlank, VirtualConstructionBlank.Factory>().FromPoolableMemoryPool();
            Container.BindFactory<IConstructionPartBlueprintSettings, IConstructionPartBlueprint,
                ConstructionPartBlueprintFactory.Factory>().FromIFactory(factory => factory.FromInstance(blueprintFactory));
            Container
                .BindFactory<ConstructionBlueprintSettings, ConstructionBlueprint,
                    ConstructionBlueprint.Factory>();
            Container.Bind<IConstructionBlueprintDatabase>().To<ConstructionBlueprintDatabase>()
                .AsSingle().WithArguments(_constructionDatabase.I);

            foreach (ConstructionBlueprintSettings construction in _constructionDatabase.I.Constructions)
            {
                foreach (IConstructionPartBlueprintSettings part in construction.PartSettings)
                {
                    blueprintFactory.InstallPools(part);
                }
            }
        }

    }
}
