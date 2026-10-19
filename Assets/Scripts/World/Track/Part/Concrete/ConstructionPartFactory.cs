using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blueprint;
using LeaseExtension.World.Track.Part.Order;
using LeaseExtension.World.Track.Part.Size;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Concrete
{
    [UsedImplicitly]
    internal class ConstructionPartFactory : IFactory<ConstructionPart>
    {
        private readonly ConstructionPartBlueprintSettings _settings;
        private readonly DiContainer _container;
        private readonly SceneContext _sceneContext;
        private Transform _poolTransform;

        public ConstructionPartFactory(ConstructionPartBlueprintSettings settings, DiContainer container,
            SceneContext sceneContext)
        {
            _settings = settings;
            _container = container;
            _sceneContext = sceneContext;
        }

        public ConstructionPart Create()
        {
            if (_poolTransform == null)
            {
                _poolTransform = new GameObject($"{_settings.PartPrefab.name} pool").transform;
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(_poolTransform.gameObject,
                    _sceneContext.gameObject.scene);
            }

            GameObject instance = _container.InstantiatePrefab(_settings.PartPrefab, _poolTransform);
            instance.SetActive(false);
            if (_settings.UseSpriteRenderer)
            {
                SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
                return _container.Instantiate<SizedConstructionPart>(
                    new object[] { instance.transform, new RendererSize(renderer), new SpriteOrder(renderer) });
            }

            if (instance.TryGetComponent(out BoxCollider2D collider))
            {
                IOrder orderer = _settings.ZOrdering ? new TransformOrder(instance.transform) : new VirtualOrder();
                return _container.Instantiate<SizedConstructionPart>(
                    new object[] { instance.transform, new ColliderSize(collider), orderer });
            }

            return _container.Instantiate<ConstructionPart>(new object[] { instance.transform });
        }
    }
}