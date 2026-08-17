using Aniki.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "ConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Construction Blueprint")]
	internal class ConstructionBlueprint : ScriptableObject, IConstructionBlueprint, IReadOnlyCollection<IConstructionPartBlueprint> {
		[SerializeField] private bool										isRangeInversed = false;
		[Tooltip("0 - use implicit height, 1 - stretch to the ceil")]
		[SerializeField, Range(0, 1)] protected float						height = 1;
		[SerializeField, Range(0, 1)] private float							rangeStart = 0;
		[SerializeField, Range(0, 1)] private float							rangeEnd = 1;
		[SerializeField] private float										minDelay = 5;
		[SerializeField] private float										maxDelay = 20;
		[SerializeField] private float										minMargin = 30;
		[SerializeField] private float										maxMargin = 50;
		[SerializeField] protected List<IRef<IConstructionPartBlueprint>>	parts;

		protected List<IConstructionBlank>	blanks;

		public IReadOnlyCollection<IConstructionPartBlueprint>	Parts => this;

		public float	MinDelay => minDelay;

		public float	MaxDelay => maxDelay;

		public float	MinMargin => minMargin;

		public float	MaxMargin => maxMargin;

		public float	RangeStart => rangeStart;

		public float	RangeEnd => rangeEnd;

		public bool	IsRangeInversed => isRangeInversed;

		public int	Count => parts.Count;

		public IEnumerator<IConstructionPartBlueprint>	GetEnumerator() {
			return new RefEnumerator<IConstructionPartBlueprint>(parts);
		}

		IEnumerator	IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public virtual IReadOnlyCollection<IConstructionBlank>	MakeBlanks() {
			blanks.Clear();
			foreach (IConstructionPartBlueprint part in this)
				blanks.Add(part.MakeBlank(0, height));
			return blanks;
		}

		private void	OnEnable() {
			if (parts != null && parts.Count > 0)
				blanks = new(parts.Count);
		}
	}
}
