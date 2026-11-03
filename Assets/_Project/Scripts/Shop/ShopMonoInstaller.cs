using LeaseExtension.Common.Service;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

namespace LeaseExtension.Shop
{
    internal class ShopMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private SpriteRenderer _wall;
        [Space, SerializeField]
        private SpriteRenderer _ruler;
        [SerializeField]
        private SpriteShapeController _rulerShadow;
        [SerializeField, Range(0, 1)]
        private float _rulerWidth = 1;
        [SerializeField, Min(0)]
        private float _rulerMinRatio;
        [SerializeField, Min(0)]
        private float _rulerMaxRatio = 1.8f;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SpriteWidthResizer>().AsCached()
                .WithArguments(_wall);
            Container.BindInterfacesTo<SpriteWidthResizer>().AsCached()
                .WithArguments(_ruler, _rulerWidth, _rulerMinRatio, _rulerMaxRatio);
            Container.BindInterfacesTo<SpriteShapeWidthResizer>().AsSingle()
                .WithArguments(_rulerShadow, _rulerWidth + 0.02f, _rulerMinRatio, _rulerMaxRatio);
        }
    }
}