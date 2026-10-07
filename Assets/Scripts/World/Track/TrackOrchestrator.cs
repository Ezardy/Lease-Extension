using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.Common.Contract;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.World.Contract;
using MessagePipe;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track
{
    [UsedImplicitly]
    internal class TrackOrchestrator : IInitializable, ITickable, IDisposable
    {
        private readonly IWorldModel _worldModel;
        private readonly IConstructionBlueprintDatabase _database;
        private readonly SortedList<float, IConstructionBlueprint> _constructionQueue;
        private readonly Dictionary<float, float> _oldToNewDistances;
        private readonly HashSet<float> _newDistances;
        private readonly List<ITrack> _tracks;
        private readonly IScreenSizeObserver _screenSizeObserver;
        private readonly ISubscriber<CharacterState> _stateSubscriber;
        private IDisposable _disposable;
        private float _distance = 0;
        private float _lastX;

        public TrackOrchestrator(
            byte trackCount,
            float floorDepth,
            float floorCenterY,
            byte maxConstructions,
            byte reservedOrders,
            IWorldModel worldModel,
            TrackFactory trackFactory,
            IConstructionBlueprintDatabase database,
            IScreenSizeObserver screenSizeObserver,
            ISubscriber<CharacterState> stateSubscriber)
        {
            _worldModel = worldModel;
            _database = database;
            _screenSizeObserver = screenSizeObserver;
            _stateSubscriber = stateSubscriber;
            trackCount = (byte)(trackCount / 2 * 2 + 1);
            float trackDepth = floorDepth / trackCount;
            float start = floorCenterY - floorDepth / 2 + trackDepth / 2;
            float scalerStep = trackCount == 1 ? 0 : (1 - worldModel.Perspective) / (trackCount - 1);
            float scalerScale = 1 / ((1 + worldModel.Perspective) / 2);
            int constructionCount = database.Constructions.Count;
            _constructionQueue = new(constructionCount);
            _oldToNewDistances = new(constructionCount);
            _newDistances = new(constructionCount);
            _tracks = new(trackCount);
            for (byte i = 0; i < trackCount; i += 1)
                _tracks.Add(trackFactory.Create(
                    start + trackDepth * i,
                    (1 - scalerStep * i) * scalerScale,
                    maxConstructions,
                    (maxConstructions + reservedOrders) * (trackCount - i - 1)));
        }

        public void Tick()
        {
            if (_worldModel.Speed > 0)
            {
                Place();
                Move();
            }
        }

        public void Initialize()
        {
            _lastX = WorldX(_screenSizeObserver.Size);
            IDisposable d1 = _screenSizeObserver.SizeChanged.Subscribe(Resize);
            IDisposable d2 = _stateSubscriber.Subscribe(Reset, CharacterStateFilter.Idle);
            IDisposable d3 = _worldModel.SpeedChanged.Subscribe(Speed);
            _disposable = Disposable.Combine(d1, d2, d3);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void Reset(CharacterState _)
        {
            foreach (ITrack track in _tracks)
                track.WipeOut();
            _constructionQueue.Clear();
            _oldToNewDistances.Clear();
            _distance = 0;
            QueueConstructions();
        }

        private void Place()
        {
            IList<float> distances = _constructionQueue.Keys;
            IList<IConstructionBlueprint> constructions = _constructionQueue.Values;
            for (int i = 0; i < distances.Count && distances[i] <= _distance; i += 1)
            {
                    PlaceConstruction(distances[i], constructions[i]);
            }
            foreach (KeyValuePair<float, float> oldNew in _oldToNewDistances)
            {
                _constructionQueue.Add(oldNew.Value, _constructionQueue[oldNew.Key]);
                _constructionQueue.Remove(oldNew.Key);
            }

            _oldToNewDistances.Clear();
            _newDistances.Clear();
        }

        private void PlaceConstruction(float distance, IConstructionBlueprint construction)
        {
            byte trackStartIndex = GetStartTrackIndex(construction);
            byte partCount = (byte)construction.Parts.Count;
            byte endIndex = (byte)(trackStartIndex + partCount);
            if (endIndex <= _tracks.Count)
            {
                bool placeable = true;
                for (byte trackIndex = trackStartIndex;
                     trackIndex < endIndex && placeable;
                     placeable = _tracks[trackIndex].IsFree, trackIndex += 1)
                    ;
                if (placeable)
                {
                    using IEnumerator<IConstructionBlank> enumerator = construction.MakeBlanks().GetEnumerator();
                    for (byte i = trackStartIndex; enumerator.MoveNext() && i < endIndex; i += 1)
                        _tracks[i].Construct(enumerator.Current);
                    _oldToNewDistances.Add(distance,
                        AddRandomDistance(construction.MinMargin, construction.MaxMargin,
                            d => !_oldToNewDistances.ContainsKey(d) && _newDistances.Add(d)));
                }
            }
        }

        private byte GetStartTrackIndex(IConstructionBlueprint construction)
        {
            float pos;
            if (construction.IsRangeInversed)
            {
                float excludedRange = construction.RangeEnd - construction.RangeStart;
                pos = UnityEngine.Random.Range(0, 1f - excludedRange);
                if (pos > construction.RangeStart)
                    pos += excludedRange;
            }
            else
            {
                pos = UnityEngine.Random.Range(construction.RangeStart, construction.RangeEnd);
            }

            return (byte)Mathf.Ceil((_tracks.Count - 1) * pos);
        }

        private void Move()
        {
            foreach (ITrack track in _tracks)
                track.Move();
            _distance += _worldModel.Speed * Time.deltaTime;
        }

        private void Speed(float speed)
        {
            foreach (ITrack track in _tracks)
                track.Speed = speed;
        }

        private void QueueConstructions()
        {
            foreach (IConstructionBlueprint construction in _database.Constructions)
                if (construction.Parts.Count <= _tracks.Count && construction.Parts.Count > 0)
                    AddRandomDistance(construction.MinDelay, construction.MaxDelay,
                        d => _constructionQueue.TryAdd(d, construction));
        }
        
        private float AddRandomDistance(float min, float max, Func<float, bool> predicate)
        {
            float newDistance = _distance + UnityEngine.Random.Range(min, max);
            if (!predicate(newDistance))
            {
                if (min == max)
                {
                    do
                    {
                        newDistance = BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(newDistance) + 1);
                    } while (!predicate(newDistance));
                }
                else
                {
                    do
                    {
                        newDistance = _distance + UnityEngine.Random.Range(min, max);
                    } while (!predicate(newDistance));
                }
            }

            return newDistance;
        }

        private void Resize(Vector2Int size)
        {
            float x = WorldX(size);
            float step = x * (1 / _worldModel.Perspective - 1) / _tracks.Count;
            float xNorm = x / _worldModel.Perspective;
            _distance += x - _lastX;
            _lastX = x;
            xNorm -= step / 2;
            foreach (ITrack track in _tracks)
            {
                track.StartX = xNorm;
                track.EndX = -x;
                xNorm -= step;
            }
        }

        private static float WorldX(Vector2Int size)
        {
            return Camera.main.orthographicSize * size.x / size.y;
        }
    }
}
