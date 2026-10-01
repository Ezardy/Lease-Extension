using System.Collections.Generic;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track
{
    internal class Track : ITrack
    {
        private readonly float _y;
        private readonly float _scaler;
        private readonly int _order;
        private readonly byte _maxConstructions;
        private readonly Queue<IConstructionPart> _parts = new();
        private float _speed;
        private float _lastPartOffset = 0;
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
            foreach (IConstructionPart part in _parts)
                part.Move(shift);
            _lastPartOffset += shift;
            WipeOutPart();
        }

        public void Construct(IConstructionBlank blank)
        {
            IConstructionPart part;
            _lastPartOrder = _parts.Count == 0 ? _order : _lastPartOrder + 1;
            part = blank.Construct(new(StartX, _y), _scaler, _lastPartOrder);
            _parts.Enqueue(part);
            _lastPartOffset = part.Blueprint.InterfereWidth * _scaler;
        }

        public void WipeOut()
        {
            foreach (IConstructionPart part in _parts)
                part.WipeOut();
            _parts.Clear();
            _lastPartOffset = 0;
        }

        private void WipeOutPart()
        {
            for (; _parts.TryPeek(out IConstructionPart part) && part.X + part.Blueprint.Width * _scaler < EndX; _parts.Dequeue(), part.WipeOut())
                ;
        }
    }
}
