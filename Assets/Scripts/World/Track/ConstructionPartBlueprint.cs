using Aniki.SceneManagment;
using MessagePipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Aniki.World {
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
					poolGameObject = new(string.Format("{0} pool", prefab.name));
					partPool = new ObjectPool<ConstructionPart>(Create, actionOnDestroy: Wipeout, defaultCapacity: initialPool);
					blankPool = new ObjectPool<ConstructionBlank>(() => new ConstructionBlank(this, partPool, blankPool));
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

		private void	Wipeout(ConstructionPart part) {
			part.Dispose();
		}

		public IConstructionBlank	MakeBlank(float height, float size) {
			ConstructionBlank	blank = blankPool.Get();

			blank.Populate(height, size);
			return blank;
		}

		private class ConstructionBlank : IConstructionBlank {
			private readonly IConstructionPartBlueprint		blueprint;
			private readonly IObjectPool<ConstructionPart>	partPool;
			private readonly IObjectPool<ConstructionBlank>	blankPool;

			private float	height;
			private float	size;

			public IConstructionPartBlueprint	Blueprint => blueprint;

			public ConstructionBlank(IConstructionPartBlueprint blueprint,
				IObjectPool<ConstructionPart> partPool,
				IObjectPool<ConstructionBlank> blankPool) {
				this.blueprint = blueprint;
				this.partPool = partPool;
				this.blankPool = blankPool;
			}

			public void	Populate(float height, float size) {
				this.height = height;
				this.size = size;
			}

			public IConstructionPart	Construct(Vector2 position, float scale, int order) {
				ConstructionPart	instance = partPool.Get();
				float				worldHeight = Camera.main.transform.position.y
					+ Camera.main.orthographicSize - position.y;

				instance.Place(position + worldHeight * height * Vector2.up,
					scale, size * worldHeight, order);
				blankPool.Release(this);
				return instance;
			}
		}

		private class ConstructionPart : IConstructionPart {
			protected readonly IConstructionPartBlueprint		blueprint;
			protected readonly IObjectPool<ConstructionPart>	pool;
			protected readonly Transform						poolTransform;
			protected readonly Transform						transform;
			protected readonly Vector3							initScale;

			public ConstructionPart(IConstructionPartBlueprint blueprint,
				Transform transform,
				IObjectPool<ConstructionPart> pool) {
				this.blueprint = blueprint;
				this.pool = pool;
				this.transform = transform;
				poolTransform = transform.parent;
				initScale = transform.transform.localScale;
			}

			public IConstructionPartBlueprint	Blueprint => blueprint;

			public float X => transform.position.x;

			public virtual void	Place(Vector3 position, float scale, float s, int order) {
				transform.parent = null;
				transform.localScale *= scale;
				transform.position = position;
				transform.gameObject.SetActive(true);
			}

			public void	WipeOut() {
				transform.parent = poolTransform;
				transform.gameObject.SetActive(false);
				transform.localScale = initScale;
				pool.Release(this);
			}

			public void	Dispose() {
				Destroy(transform.gameObject);
			}

			public void	Move(float shift) {
				transform.Translate(shift, 0, 0);
			}
		}

		private class SizedConstructionPart : ConstructionPart {
			private readonly ISize	sizer;
			private readonly IOrder	orderer;

			public SizedConstructionPart(IConstructionPartBlueprint blueprint,
				Transform transform, ISize sizer,
				IObjectPool<ConstructionPart> pool, IOrder orderer)
				: base(blueprint, transform, pool) {
				this.sizer = sizer;
				this.orderer = orderer;
			}

			public override void Place(Vector3 position, float scale, float s, int order) {
				base.Place(position, scale, s, order);
				if (s != 0)
					sizer.Size = new(sizer.Size.x, s / transform.localScale.y);
				transform.position += position - sizer.Min + blueprint.Margin * Vector3.right;
				orderer.Order = order;
			}
		}

		private interface IOrder {
			public int	Order { get; set; }
		}

		private class SpriteOrder : IOrder {
			private readonly SpriteRenderer	renderer;

			public int	Order {
				get => renderer.sortingOrder;
				set => renderer.sortingOrder = value;
			}

			public SpriteOrder(SpriteRenderer renderer) {
				this.renderer = renderer;
			}
		}

		private class TransformOrder : IOrder {
			private readonly Transform	transform;

			public int	Order {
				get => (int)transform.position.z;
				set {
					Vector3	position = transform.position;

					position.z = value;
					transform.position = position;
				}
			}

			public TransformOrder(Transform transform) {
				this.transform = transform;
			}
		}

		private class VirtualOrder : IOrder {
			public int	Order { get; set; }
		}

		private interface ISize {
			public Vector2	Size { get; set; }
			public Vector3	Min { get; }
		}

		private class RendererSize : ISize {
			private readonly SpriteRenderer	renderer;

			public Vector2	Size {
				get => renderer.size;
				set => renderer.size = value;
			}

			public Vector3	Min => renderer.bounds.min;

			public RendererSize(SpriteRenderer renderer) {
				this.renderer = renderer;
			}
		}

		private class ColliderSize : ISize {
			private readonly BoxCollider2D	collider;

			public Vector2	Size {
				get => collider.size;
				set => collider.size = value;
			}

			public Vector3	Min => collider.bounds.min;

			public ColliderSize(BoxCollider2D collider) {
				this.collider = collider;
			}
		}
	}
}
