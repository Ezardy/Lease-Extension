using Aniki.SceneManagment;
using MessagePipe;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "VirtualConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Virtual Construction Part Blueprint")]
	internal class VirtualConstructionPartBlueprint : ScriptableObject, IConstructionPartBlueprint, IConstructionBlank {
		[SerializeField] private byte	initialPool = 5;
		[SerializeField] private float	width = 1;

		private IObjectPool<ConstructionPart>	pool;
		private System.IDisposable				disposable;

		public IReadOnlyCollection<IConstructionPartBlueprint>	SubPartBlueprints => System.Array.Empty<IConstructionPartBlueprint>();

		public float	InterfereWidth => width;
		public float	Width => 0;
		public float	Margin => 0;

		public IConstructionPart	Construct(Vector2 position, float scale, int order) {
			ConstructionPart	part = pool.Get();

			part.Move(position.x);
			return part;
		}

		[Inject]
		public void	Init(ISubscriber<FocusedScene> sceneSubscriber) {
			disposable = sceneSubscriber.Subscribe(_ =>
				pool ??= new ObjectPool<ConstructionPart>(Create,
					actionOnRelease: Release, defaultCapacity: initialPool),
				FocusedSceneFilter.Main);
		}

		private void	OnDisable() {
			disposable?.Dispose();
			disposable = null;
		}

		private ConstructionPart	Create() {
			return new ConstructionPart(this);
		}

		private void	Release(ConstructionPart part) {
			part.Move(-part.X);
		}

		public IConstructionBlank	MakeBlank(float height, float size) {
			return this;
		}

		private class ConstructionPart : IConstructionPart {
			private readonly IConstructionPartBlueprint	blueprint;

			private float	x = 0;

			public ConstructionPart(IConstructionPartBlueprint blueprint) {
				this.blueprint = blueprint;
			}

			public IConstructionPartBlueprint	Blueprint => blueprint;

			public float	X => x;

			public void	Move(float shift) {
				x += shift;
			}

			public void	WipeOut() { }
		}
	}
}
