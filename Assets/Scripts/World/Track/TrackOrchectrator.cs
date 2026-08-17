using Aniki.Character;
using Aniki.UI;
using MessagePipe;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class TrackOrchectrator : IInitializable, ITickable, IDisposable {
		private readonly IWorldModel								worldModel;
		private readonly IConstructionBlueprintDatabase				database;
		private readonly SortedList<float, IConstructionBlueprint>	constructionQueue;
		private readonly Dictionary<float, float>					oldToNewDistances;
		private readonly List<ITrack>								tracks;
		private readonly IScreenSizeObserver						screenSizeObserver;
		private readonly ISubscriber<CharacterState>				stateSubscriber;

		private IDisposable	disposable;

		private float	distance = 0;
		private float	lastX;

		public void	Tick() {
			if (worldModel.Speed > 0) {
				Place();
				Move();
			}
		}

		private void	Place() {
			foreach (KeyValuePair<float, IConstructionBlueprint> pair in constructionQueue) {
				if (pair.Key <= distance)
					PlaceConstruction(pair.Key, pair.Value);
			}
			foreach (KeyValuePair<float, float> oldNew in oldToNewDistances) {
				constructionQueue.Add(oldNew.Value, constructionQueue[oldNew.Key]);
				constructionQueue.Remove(oldNew.Key);
			}
			oldToNewDistances.Clear();
		}

		private void	PlaceConstruction(float distance, IConstructionBlueprint construction) {
			byte	trackStartIndex = GetStartTrackIndex(construction);
			byte	partCount = (byte)construction.Parts.Count;
			byte	endIndex = (byte)(trackStartIndex + partCount);

			if (endIndex <= tracks.Count) {
				bool	placable = true;

				for (byte trackIndex = trackStartIndex;
					trackIndex < endIndex && placable;
					placable = tracks[trackIndex].IsFree, trackIndex += 1);
				if (placable) {
					IEnumerator<IConstructionBlank>	enumerator = construction.MakeBlanks().GetEnumerator();

					for (byte i = trackStartIndex;
						enumerator.MoveNext() && i < endIndex; i += 1)
						tracks[i].Construct(enumerator.Current);
					while (!oldToNewDistances.TryAdd(distance,
							this.distance + UnityEngine.Random.Range(construction.MinMargin, construction.MaxMargin)));
				}
			}
		}

		private byte	GetStartTrackIndex(IConstructionBlueprint construction) {
			float	pos;

			if (construction.IsRangeInversed) {
				float	excludedRange = construction.RangeEnd - construction.RangeStart;

				pos = UnityEngine.Random.Range(0, 1f - excludedRange);
				if (pos > construction.RangeStart)
					pos += excludedRange;
			} else
				pos = UnityEngine.Random.Range(construction.RangeStart, construction.RangeEnd);
			return (byte)Mathf.Ceil((tracks.Count - 1) * pos);
		}

		private void	Move() {
			foreach (ITrack track in tracks)
				track.Move();
			distance += worldModel.Speed * Time.deltaTime;
		}

		private void	Speed(float speed) {
			foreach (ITrack track in tracks)
				track.Speed = speed;
		}

		public TrackOrchectrator(byte trackCount, float floorDepth,
			float floorCenterY, byte maxConstructons, byte reservedOrders,
			IWorldModel worldModel, ITrackFactory trackFactory,
			IConstructionBlueprintDatabase database,
			IScreenSizeObserver screenSizeObserver,
			ISubscriber<CharacterState> stateSubscriber) {
			this.worldModel = worldModel;
			this.database = database;
			this.screenSizeObserver = screenSizeObserver;
			this.stateSubscriber = stateSubscriber;
			trackCount = (byte)(trackCount / 2 * 2 + 1);

			float	trackDepth = floorDepth / trackCount;
			float	start = floorCenterY - floorDepth / 2 + trackDepth / 2;
			float	scalerStep = trackCount == 0 ? 0 : (1 - worldModel.Perspective) / (trackCount - 1);
			float	scalerScale = 1 / ((1 + worldModel.Perspective) / 2);
			int		constructionCount = database.Constructions.Count;

			constructionQueue = new(constructionCount);
			oldToNewDistances = new(constructionCount);
			tracks = new(trackCount);

			for (byte i = 0; i < trackCount; i += 1)
				tracks.Add(trackFactory.Create(
						start + trackDepth * i,
						(1 - scalerStep * i) * scalerScale, maxConstructons,
						(maxConstructons + reservedOrders) * (trackCount - i - 1)));
		}

		public void	Initialize() {
			lastX = WorldX(screenSizeObserver.Size);

			IDisposable	d1 = screenSizeObserver.SizeChanged.Subscribe(Resize);
			IDisposable	d2 = stateSubscriber.Subscribe(Reset, CharacterStateFilter.Idle);
			IDisposable	d3 = worldModel.SpeedChanged.Subscribe(Speed);

			disposable = Disposable.Combine(d1, d2, d3);
		}

		private void	QueueConstructions() {
			foreach (IConstructionBlueprint construction in database.Constructions) {
				if (construction.Parts.Count <= tracks.Count && construction.Parts.Count > 0)
					while (!constructionQueue.TryAdd(
						UnityEngine.Random.Range(construction.MinDelay,
							construction.MaxDelay), construction));
			}
		}

		private void	Resize(Vector2Int size) {
			float	x = WorldX(size);
			float	step = x * (1 / worldModel.Perspective - 1) / tracks.Count;
			float	xNorm = x / worldModel.Perspective;

			distance += x - lastX;
			lastX = x;
			xNorm -= step / 2;
			foreach (ITrack track in tracks) {
				track.StartX = xNorm;
				//track.EndX = -xNorm;
				track.EndX = -x;
				xNorm -= step;
			}
		}

		private void	Reset(CharacterState _) {
			foreach (ITrack track in tracks)
				track.WipeOut();
			constructionQueue.Clear();
			oldToNewDistances.Clear();
			distance = 0;
			QueueConstructions();
		}

		public void	Dispose() {
			disposable.Dispose();
		}

		private static float	WorldX(Vector2Int size) {
			return Camera.main.orthographicSize * size.x / size.y;
		}
	}
}
