using System.Collections.Generic;
using Aniki.SceneManagment;
using Aniki.World;
using LeaseExtension.World.Entities.Internal;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;
using LeaseExtension.World.Track.Part.Order;
using LeaseExtension.World.Track.Part.Size;
using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint {
	[CreateAssetMenu(fileName = "ConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Construction Part Blueprint")]
	internal class ConstructionPartBlueprint : ScriptableObject, IConstructionPartBlueprint {
		[SerializeField] private byte	initialPool = 5;

		[Space]

		[SerializeField] private float	margin = 0;
		[SerializeField] private float	width = 0;
		[SerializeField] private bool	autoWidth = true;

		[Space]

		[SerializeField] private float	interfereWidth = 0;
		[SerializeField] private bool	autoInterfereWidth = true;
		[SerializeField] private bool	sameWidth = true;

		[Space]

		[SerializeField] private bool			useSpriteRenderer = true;
		[SerializeField] private bool			zOrdering = false;
		[SerializeField] protected GameObject	prefab;

		private IObjectPool<ConstructionPart>	partPool;
		private IObjectPool<ConstructionBlank>	blankPool;
		private GameObject						poolGameObject;
		private System.IDisposable				disposable;

		public IReadOnlyCollection<IConstructionPartBlueprint>	SubPartBlueprints => System.Array.Empty<IConstructionPartBlueprint>();

		public float	InterfereWidth => interfereWidth + margin;
		public float	Width => width + margin;
		public float	Margin => margin;

		private void	OnEnable() {
			float	aWidth = 0;

			if (prefab != null) {
				if (autoWidth || (!sameWidth && autoInterfereWidth)) {
					aWidth = useSpriteRenderer
						? prefab.GetComponent<SpriteRenderer>().bounds.size.x
						: prefab.GetComponent<BoxCollider2D>().bounds.size.x;
				}
				if (autoWidth)
					width = aWidth;
				if (sameWidth)
					interfereWidth = width;
				else if (autoInterfereWidth)
					interfereWidth = aWidth;
			}
		}

		[Inject]
		public void	Construct(ISubscriber<FocusedScene> sceneSubscriber) {
			disposable = sceneSubscriber.Subscribe(_ => {
				if (poolGameObject == null) {
					poolGameObject = new($"{prefab.name} pool");
					partPool = new ObjectPool<ConstructionPart>(Create, actionOnDestroy: Wipeout, defaultCapacity: initialPool);
					blankPool = new ObjectPool<ConstructionBlank>(() => new(this, partPool, blankPool));
				}
			}, FocusedSceneFilter.Main);
		}

		private void	OnDisable() {
			disposable?.Dispose();
			disposable = null;
		}

		private ConstructionPart	Create() {
			GameObject			instance = CreateInstance();
			ISize				sizer;
			IOrder				orderer;
			ConstructionPart	part;

			instance.transform.parent = poolGameObject.transform;
			instance.SetActive(false);
			if (useSpriteRenderer) {
				SpriteRenderer	renderer = instance.GetComponent<SpriteRenderer>();

				sizer = new RendererSize(renderer);
				orderer = new SpriteOrder(renderer);
				part = new SizedConstructionPart(this, instance.transform, sizer,
					partPool, orderer);
			} else if (instance.TryGetComponent(out BoxCollider2D collider)) {
				sizer = new ColliderSize(collider);
				if (zOrdering)
					orderer = new TransformOrder(instance.transform);
				else
					orderer = new VirtualOrder();
				part = new SizedConstructionPart(this, instance.transform, sizer,
					partPool, orderer);
			} else
				part = new(this, instance.transform, partPool);
			return part;
		}

		protected virtual GameObject	CreateInstance() {
			return Instantiate(prefab);
		}

		private static void	Wipeout(ConstructionPart part) {
			part.Dispose();
		}

		public IConstructionBlank	MakeBlank(float height, float size) {
			ConstructionBlank	blank = blankPool.Get();

			blank.Populate(height, size);
			return blank;
		}
	}
}
