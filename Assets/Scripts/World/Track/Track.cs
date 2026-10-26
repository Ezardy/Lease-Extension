using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track
{
    [UsedImplicitly]
    internal class Track : ITrack
    {
        private readonly float _y;
        private readonly float _scaler;
        private readonly int _order;
        private readonly byte _maxConstructions;
        private readonly SortedList<float, IConstructionPart> _parts = new();
        private float _speed;
        private float _lastPartOffset;
        private float _firstPartMovedDistance;
        private int _lastPartOrder;

        public float StartX { get; set; }
        public float EndX { get; set; }
        public bool IsFree => _lastPartOffset <= 0 && _parts.Count <= _maxConstructions;

        public float Speed
        {
            get => _speed;
            set => _speed = value * _scaler;
        }

        public Track(float y, float scaler, byte maxConstructions, int order)
        {
            _y = y;
            _scaler = scaler;
            _maxConstructions = maxConstructions;
            _order = order;
            _lastPartOrder = order;
        }

        public void Move()
        {
            float shift = -_speed * Time.deltaTime;
            foreach (KeyValuePair<float, IConstructionPart> part in _parts)
            {
                part.Value.Move(shift);
            }

            _firstPartMovedDistance = _parts.Count > 0 ? _firstPartMovedDistance - shift : 0;
            _lastPartOffset += shift;
            WipeOutPart();
        }

        public void Construct(IConstructionBlank blank)
        {
            _lastPartOrder = _parts.Count == 0 ? _order : _lastPartOrder + 1;
            IConstructionPart part = blank.Construct(new(StartX, _y), _scaler, _lastPartOrder);
            float tail = part.Blueprint.Width * _scaler + _firstPartMovedDistance;
            while (!_parts.TryAdd(tail, part))
            {
                tail = BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(tail) + 1);
            }
            _lastPartOffset = part.Blueprint.InterfereWidth * _scaler;
        }

        public void WipeOut()
        {
            foreach (KeyValuePair<float, IConstructionPart> part in _parts)
            {
                part.Value.WipeOut();
            }

            _parts.Clear();
            _lastPartOffset = 0;
        }

        private void WipeOutPart()
        {
            while (_parts.Count > 0 && _parts.Values[0] is {X: var x, Blueprint: { Width: var width } } part && x + width * _scaler < EndX)
            {
                part.WipeOut();
                _parts.RemoveAt(0);
            }
        }
    }
}