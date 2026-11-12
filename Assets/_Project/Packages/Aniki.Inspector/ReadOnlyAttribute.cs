using System;
using UnityEngine;

namespace Aniki.Inspector
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ReadOnlyAttribute : PropertyAttribute
    {
        public ReadOnlyAttribute() : base(true) {}
    }
}
