using LeaseExtension.Common.Contract;
using UnityEngine;

namespace LeaseExtension.Cosmetics.Contract
{
    public interface ICosmeticsModel : IIdentifiable
    {
        public Sprite Back { get; }
        public Sprite Front { get; }
    }
}