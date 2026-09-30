using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace LeaseExtension.Cosmetics
{
    [Serializable]
    internal struct CosmeticsCollection
    {
        [FormerlySerializedAs("idCollection")]
        public List<string> IdCollection;
    }
}
