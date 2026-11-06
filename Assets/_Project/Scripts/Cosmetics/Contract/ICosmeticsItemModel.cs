using Aniki.Common;
using UnityEngine;

namespace LeaseExtension.Cosmetics.Contract
{
    public interface ICosmeticsItemModel : IIdentifiable
    {
        public uint Price { get; }
        public Sprite Back { get; }
        public Sprite Front { get; }
    }
}
