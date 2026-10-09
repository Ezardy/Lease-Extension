using System.Collections;
using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "ConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Construction Blueprint")]
    internal class ConstructionBlueprint : ScriptableObject, IConstructionBlueprint, IReadOnlyCollection<IConstructionPartBlueprint>
    {
        [SerializeField, Tooltip("0 - use implicit height, 1 - stretch to the ceil"), Range(0, 1)]
        protected float Height = 1;
        [SerializeField]
        protected List<Ref<IConstructionPartBlueprint>> PartReferences;
        protected List<IConstructionBlank> Blanks;
        [SerializeField]
        private bool _isRangeInversed = false;
        [SerializeField, Range(0, 1)]
        private float _rangeStart = 0;
        [SerializeField, Range(0, 1)]
        private float _rangeEnd = 1;
        [SerializeField]
        private float _minDelay = 5;
        [SerializeField]
        private float _maxDelay = 20;
        [SerializeField]
        private float _minMargin = 30;
        [SerializeField]
        private float _maxMargin = 50;

        public IReadOnlyCollection<IConstructionPartBlueprint> Parts => this;
        public float MinDelay => _minDelay;
        public float MaxDelay => _maxDelay;
        public float MinMargin => _minMargin;
        public float MaxMargin => _maxMargin;
        public float RangeStart => _rangeStart;
        public float RangeEnd => _rangeEnd;
        public bool IsRangeInversed => _isRangeInversed;
        public int Count => PartReferences.Count;

        private void OnEnable()
        {
            if (PartReferences != null && PartReferences.Count > 0)
                Blanks = new(PartReferences.Count);
        }

        public IEnumerator<IConstructionPartBlueprint> GetEnumerator()
        {
            return new RefEnumerator<IConstructionPartBlueprint>(PartReferences);
        }

        public virtual IReadOnlyCollection<IConstructionBlank> MakeBlanks()
        {
            Blanks.Clear();
            foreach (IConstructionPartBlueprint part in this)
                Blanks.Add(part.MakeBlank(0, Height));
            return Blanks;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
