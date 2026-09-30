using System.Collections.Generic;
using Aniki.SceneManagment;
using Aniki.World;
using LeaseExtension.World.Track.Part.Concrete;
using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint {
	[CreateAssetMenu(fileName = "VirtualConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Virtual Construction Part Blueprint")]
	internal class VirtualConstructionPartBlueprint : ScriptableObject, IConstructionPartBlueprint, IConstructionBlank {
		[SerializeField] private byte	initialPool = 5;
		[SerializeField] private float	width = 1;

		private IObjectPool<VirtualConstructionPart>	pool;
		private System.IDisposable				disposable;

		public IReadOnlyCollection<IConstructionPartBlueprint>	SubPartBlueprints => System.Array.Empty<IConstructionPartBlueprint>();

		public float	InterfereWidth => width;
		public float	Width => 0;
		public float	Margin => 0;

		public IConstructionPart	Construct(Vector2 position, float scale, int order) {
			VirtualConstructionPart	part = pool.Get();

			part.Move(position.x);
			return part;
		}

		[Inject]
		public void	Init(ISubscriber<FocusedScene> sceneSubscriber) {
			disposable = sceneSubscriber.Subscribe(_ =>
				pool ??= new ObjectPool<VirtualConstructionPart>(Create,
					actionOnRelease: Release, defaultCapacity: initialPool),
				FocusedSceneFilter.Main);
		}

		private void	OnDisable() {
			disposable?.Dispose();
			disposable = null;
		}

		private VirtualConstructionPart	Create() {
			return new VirtualConstructionPart(this);
		}

		private void	Release(VirtualConstructionPart part) {
			part.Move(-part.X);
		}

		public IConstructionBlank	MakeBlank(float height, float size) {
			return this;
		}
	}
}
