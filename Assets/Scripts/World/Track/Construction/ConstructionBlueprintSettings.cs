using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blueprint;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "ConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Construction Blueprint")]
    internal class ConstructionBlueprintSettings : ScriptableObject
    {
        [SerializeField, Tooltip("0 - use implicit height, 1 - stretch to the ceil"), Range(0, 1)]
        protected float Height = 1;
        [SerializeField]
        protected List<Ref<IConstructionPartBlueprintSettings>> PartReferences;
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

        public IEnumerable<IConstructionPartBlueprintSettings> PartSettings
        {
            get
            {
                foreach (Ref<IConstructionPartBlueprintSettings> part in PartReferences)
                    yield return part.I;
            }
        }
        public float MinDelay => _minDelay;
        public float MaxDelay => _maxDelay;
        public float MinMargin => _minMargin;
        public float MaxMargin => _maxMargin;
        public float RangeStart => _rangeStart;
        public float RangeEnd => _rangeEnd;
        public bool IsRangeInversed => _isRangeInversed;

        public virtual void MakeBlanks(IEnumerable<IConstructionPartBlueprint> parts, ICollection<IConstructionBlank> blanksOutput)
        {
            foreach (IConstructionPartBlueprint part in parts)
                blanksOutput.Add(part.MakeBlank(0, Height));
        }
    }
}
