using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.World.Ceil;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Floor;
using LeaseExtension.World.Track;
using LeaseExtension.World.Track.Part.Behaviour;
using LeaseExtension.World.Wall;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World
{
    internal class WorldMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private SpriteRenderer _floorSpriteRenderer;
        [SerializeField]
        private Animator _floorAnimator;
        [SerializeField]
        private MonoBehaviour _floorViewModel;
        [SerializeField]
        private SpriteRenderer _wallSpriteRenderer;
        [SerializeField, Space]
        private byte _trackCount = 11;
        [SerializeField]
        private byte _maxConstructionCount = 100;
        [SerializeField]
        private byte _reservedOrderCount = 5;
        [SerializeField, Space]
        private IRef<IWorldModel> _worldModel;
        [SerializeField]
        private IRef<IConstructionBlueprintDatabase> _constructionDatabase;

        public override void InstallBindings()
        {
            InstallWall();
            InstallFloor();
            InstallTracks();
            Container.BindInstance(_worldModel.I);
            Container.QueueForInject(_worldModel.I);
            Container.BindInstance(_constructionDatabase.I);
            Container.Bind<INoPunchZone>().To<NoPunchZoneManager>().AsSingle();
            Container.BindInterfacesTo<WorldViewModel>().AsSingle();
            Container.BindFactory<float, float, byte, int, ITrack, ITrackFactory>().To<LeaseExtension.World.Track.Track>();
            Container.BindFactory<Object, AConstructionPartBehaviour, AConstructionPartBehaviour.Factory>().FromFactory<PrefabFactory<AConstructionPartBehaviour>>();
            InstallPartBlueprints();
        }

        private void InstallTracks()
        {
            float depth = _floorSpriteRenderer.bounds.size.y;
            float centerY = _floorSpriteRenderer.transform.position.y;
            Container.BindInterfacesTo<TrackOrchectrator>().AsSingle().WithArguments(
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
            HashSet<object> uniqueParts = new();
            foreach (IConstructionBlueprint construction in _constructionDatabase.I.Constructions)
                foreach (IConstructionPartBlueprint part in construction.Parts)
                {
                    uniqueParts.Add(part);
                    foreach (IConstructionPartBlueprint subPart in part.SubPartBlueprints)
                        uniqueParts.Add(subPart);
                }

            foreach (object part in uniqueParts)
                Container.QueueForInject(part);
        }
    }
}
