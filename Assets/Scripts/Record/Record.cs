using System;
using UnityEngine.Serialization;

namespace LeaseExtension.Record
{
    [Serializable]
    internal struct Record
    {
        [FormerlySerializedAs("record")]
        public uint Value;
    }
}
