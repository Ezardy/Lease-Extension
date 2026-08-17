using Aniki.Common;
using Aniki.World;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

internal class WorldMonoInstaller : MonoInstaller {
	[SerializeField] private SpriteRenderer	floorSpriteRenderer;
	[SerializeField] private Animator		floorAnimator;
	[SerializeField] private MonoBehaviour	floorViewModel;
	[SerializeField] private SpriteRenderer	wallSpriteRenderer;

	[Space]

	[SerializeField] private byte	trackCount = 11;
	[SerializeField] private byte	maxConstructionCount = 100;
	[SerializeField] private byte	reservedOrderCount = 5;

	[Space]

	[SerializeField] private IRef<IWorldModel>						worldModel;
	[SerializeField] private IRef<IConstructionBlueprintDatabase>	constructionDatabase;

	public override void	InstallBindings() {
		InstallWall();
		InstallFloor();
		InstallTracks();

		Container.BindInstance(worldModel.I);
		Container.QueueForInject(worldModel.I);
		Container.BindInstance(constructionDatabase.I);
		Container.Bind<INoPunchZone>().To<NoPunchZoneManager>().AsSingle();
		Container.BindInterfacesTo<WorldViewModel>().AsSingle();
		Container.BindFactory<float, float, byte, int, ITrack, ITrackFactory>().To<Track>();
		Container.BindFactory<Object, AConstructionPartBehaviour, AConstructionPartBehaviour.Factory>().FromFactory<PrefabFactory<AConstructionPartBehaviour>>();

		InstallPartBlueprints();
	}

	private void	InstallTracks() {
		float	depth = floorSpriteRenderer.bounds.size.y;
		float	centerY = floorSpriteRenderer.transform.position.y;


		Container.BindInterfacesTo<TrackOrchectrator>().AsSingle()
			.WithArguments(trackCount, depth, centerY, maxConstructionCount,
				reservedOrderCount);
	}

	private void	InstallWall() {
		Container.BindInterfacesTo<WallView>().AsSingle().WithArguments(wallSpriteRenderer);
		Container.BindInterfacesTo<WallViewModel>().AsSingle();
	}

	private void	InstallFloor() {
		Container.BindInstance(floorAnimator).WhenInjectedInto<FloorViewModel>();
		Container.BindInterfacesTo<FloorView>().AsSingle().WithArguments(floorSpriteRenderer);
		Container.QueueForInject(floorViewModel);
	}

	private void	InstallPartBlueprints() {
		HashSet<object>	uniqueParts = new();

		foreach (IConstructionBlueprint construction in constructionDatabase.I.Constructions) {
			foreach (IConstructionPartBlueprint part in construction.Parts) {
				uniqueParts.Add(part);
				foreach (IConstructionPartBlueprint subPart in part.SubPartBlueprints)
					uniqueParts.Add(subPart);
			}
		}
		foreach (object part in uniqueParts)
			Container.QueueForInject(part);
	}
}
