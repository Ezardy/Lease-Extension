using Aniki.Common;
using Aniki.SceneManagment;
using MessagePipe;
using System;
using System.Collections.Generic;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;
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

		private IObjectPool<GapConstructionPart>	partPool;
		private IObjectPool<GapConstructionBlank>	blankPool;
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
			GapConstructionBlank	blank = blankPool.Get();

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
					blankPool = new ObjectPool<GapConstructionBlank>(() =>
						new(this, blankPool, partPool),
						actionOnRelease: b => b.Depopulate());
					partPool = new ObjectPool<GapConstructionPart>(() =>
						new(this, partPool), actionOnRelease: p =>
							p.Depopulate());
				}
			}, FocusedSceneFilter.Main);
		}

		private void	OnDisable() {
			disposable?.Dispose();
			disposable = null;
		}
	}
}
