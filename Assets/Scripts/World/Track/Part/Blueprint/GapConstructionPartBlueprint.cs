using Aniki.Common;
using Aniki.SceneManagment;
using MessagePipe;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "GapConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Gap Construction Part Blueprint")]
	internal class GapConstructionPartBlueprint : ScriptableObject, IGapConstructionPartBlueprint {
		[SerializeField, Range(0, 1)] private float					gapSize;
		[SerializeField] private IRef<IConstructionPartBlueprint>	bottomPart;
		[SerializeField] private IRef<IConstructionPartBlueprint>	gapPart;
		[SerializeField] private IRef<IConstructionPartBlueprint>	topPart;

		private IObjectPool<ConstructionPart>	partPool;
		private IObjectPool<ConstructionBlank>	blankPool;
		private IConstructionPartBlueprint[]	subPartBlueprints;

		private float		width;
		private float		margin;
		private float		interfereWidth;
		private IDisposable	disposable;

		public IReadOnlyCollection<IConstructionPartBlueprint>	SubPartBlueprints => subPartBlueprints;

		public float	Width => width;
		public float	Margin => margin;
		public float	GapSize => gapSize;
		public float	InterfereWidth => interfereWidth;

		public IConstructionBlank	MakeBlank(float height, float gapPos) {
			float				gapStart = gapPos - gapSize / 2 + height;
			ConstructionBlank	blank = blankPool.Get();

			blank.Populate(bottomPart.I.MakeBlank(height, gapStart),
			gapPart.I.MakeBlank(gapStart, gapSize),
			topPart.I.MakeBlank(gapStart + gapSize, 1 - gapStart - gapSize));
			return blank;
		}

		private void	OnEnable() {
			if (bottomPart && gapPart && topPart) {
				width = Mathf.Max(bottomPart.I.Width, gapPart.I.Width, topPart.I.Width);
				margin = Mathf.Max(bottomPart.I.Margin, gapPart.I.Margin, topPart.I.Margin);
				interfereWidth = Mathf.Max(bottomPart.I.InterfereWidth, gapPart.I.InterfereWidth, topPart.I.InterfereWidth);
				subPartBlueprints = new IConstructionPartBlueprint[3] {
					bottomPart.I, gapPart.I, topPart.I
				};
			}
		}

		[Inject]
		public void	Construct(ISubscriber<FocusedScene> sceneSubscriber) {
			disposable = sceneSubscriber.Subscribe(_ => {
				if (blankPool == null) {
					blankPool = new ObjectPool<ConstructionBlank>(() =>
						new ConstructionBlank(this, blankPool, partPool),
						actionOnRelease: b => b.Depopulate());
					partPool = new ObjectPool<ConstructionPart>(() =>
						new ConstructionPart(this, partPool), actionOnRelease: p =>
							p.Depopulate());
				}
			}, FocusedSceneFilter.Main);
		}

		private void	OnDisable() {
			disposable?.Dispose();
			disposable = null;
		}

		private class ConstructionBlank : IConstructionBlank {
			private readonly IConstructionPartBlueprint		blueprint;
			private readonly IObjectPool<ConstructionBlank>	blankPool;
			private readonly IObjectPool<ConstructionPart>	partPool;

			private IConstructionBlank	bottomBlank;
			private IConstructionBlank	gapBlank;
			private IConstructionBlank	topBlank;

			public IConstructionPartBlueprint	Blueprint => blueprint;

			public ConstructionBlank(IConstructionPartBlueprint blueprint,
				IObjectPool<ConstructionBlank> blankPool,
				IObjectPool<ConstructionPart> partPool) {
				this.blueprint = blueprint;
				this.blankPool = blankPool;
				this.partPool = partPool;
			}

			public void	Populate(IConstructionBlank bottomBlank,
				IConstructionBlank gapBlank,
				IConstructionBlank topBlank) {
				this.bottomBlank = bottomBlank;
				this.gapBlank = gapBlank;
				this.topBlank = topBlank;
			}

			public void	Depopulate() {
				bottomBlank = null;
				gapBlank = null;
				topBlank = null;
			}

			public IConstructionPart	Construct(Vector2 position,
				float scale, int order) {
				ConstructionPart	part = partPool.Get();

				part.Populate(this, bottomBlank.Construct(position, scale, order),
					gapBlank.Construct(position, scale, order),
					topBlank.Construct(position, scale, order));
				WipeOut();
				return part;
			}

			private void	WipeOut() {
				blankPool.Release(this);
			}
		}

		private class ConstructionPart : IConstructionPart {
			private readonly IObjectPool<ConstructionPart>	pool;
			private readonly IConstructionPartBlueprint		blueprint;

			private IConstructionPart				bottomPart;
			private IConstructionPart				gapPart;
			private IConstructionPart				topPart;

			public ConstructionPart(IConstructionPartBlueprint blueprint,
				IObjectPool<ConstructionPart> pool) {
				this.blueprint = blueprint;
				this.pool = pool;
			}

			public IConstructionPartBlueprint	Blueprint => blueprint;

			public void	Populate(IConstructionBlank blank,
				IConstructionPart bottomPart, IConstructionPart gapPart,
				IConstructionPart topPart) {
				this.bottomPart = bottomPart;
				this.gapPart = gapPart;
				this.topPart = topPart;
			}

			public void	Depopulate() {
				bottomPart = null;
				gapPart = null;
				topPart = null;
			}

			public float	X => bottomPart.X;

			public void	Move(float shift) {
				bottomPart.Move(shift);
				gapPart.Move(shift);
				topPart.Move(shift);
			}

			public void	WipeOut() {
				bottomPart.WipeOut();
				gapPart.WipeOut();
				topPart.WipeOut();
				pool.Release(this);
			}
		}
	}
}
