using LeaseExtension.Common.Contract;
using UnityEngine;

namespace LeaseExtension.Shop.Contract
{
    public interface IProductModel : IIdentifiable
    {
        public string Label { get; }
        public uint Price { get; }
        public Texture2D BackPreview { get; }
        public Texture2D FrontPreview { get; }
    }
}