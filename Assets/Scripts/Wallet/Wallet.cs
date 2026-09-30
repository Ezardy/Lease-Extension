using System;
using UnityEngine.Serialization;

namespace LeaseExtension.Wallet
{
    [Serializable]
    internal struct Wallet
    {
        [FormerlySerializedAs("balance")]
        public uint Balance;
    }
}
