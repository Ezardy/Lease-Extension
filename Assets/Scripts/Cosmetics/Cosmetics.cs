using System;
using UnityEngine.Serialization;

namespace LeaseExtension.Cosmetics
{
    [Serializable]
    internal struct Cosmetics
    {
        [FormerlySerializedAs("id")]
        public string Id;
    }
}
