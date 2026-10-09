using UnityEngine;

namespace LeaseExtension.World.Contract
{
    public interface IConstructionBlank
    {
        public IConstructionPart Construct(Vector2 position, float scale, int order);
    }
}
