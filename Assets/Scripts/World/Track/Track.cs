using System.Collections.Generic;
using UnityEngine;

namespace Aniki.World {
	internal class Track : ITrack {
		private readonly float						y;
		private readonly float						scaler;
		private readonly int						order;
		private readonly byte						maxConstructions;
		private readonly Queue<IConstructionPart>	parts = new();

		private float	speed;
		private float	lastPartOffset = 0;
		private int		lastPartOrder;

		public float	StartX { get; set; }
		public float	EndX { get; set; }

		public bool	IsFree => lastPartOffset <= 0 && parts.Count <= maxConstructions;

		public float	Speed {
			get => speed;
			set => speed = value * scaler;
		}

		public void	Move() {
			float	shift = -speed * Time.deltaTime;

			foreach (IConstructionPart part in parts)
				part.Move(shift);
			lastPartOffset += shift;
			WipeOutPart();
		}

		public void	Construct(IConstructionBlank blank) {
			IConstructionPart	part;

			lastPartOrder = parts.Count == 0 ? order : lastPartOrder + 1;
			part = blank.Construct(new(StartX, y), scaler, lastPartOrder);
			parts.Enqueue(part);
			lastPartOffset = part.Blueprint.InterfereWidth * scaler;
		}

		public void	WipeOut() {
			foreach (IConstructionPart part in parts)
				part.WipeOut();
			parts.Clear();
			lastPartOffset = 0;
		}

		public Track (float y, float scaler, byte maxConstructions, int order) {
			this.y = y;
			this.scaler = scaler;
			this.maxConstructions = maxConstructions;
			this.order = order;
			lastPartOrder = order;
		}

		private void	WipeOutPart() {
			for (; parts.TryPeek(out IConstructionPart part)
				&& part.X + part.Blueprint.Width * scaler < EndX;
				parts.Dequeue(), part.WipeOut());
		}
	}
}
