using System.Collections.Generic;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;
using LeaseExtension.World.Track.Part.Order;
using LeaseExtension.World.Track.Part.Size;
using R3;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "ConstructionPartBlueprint",
        menuName = "Scriptable Objects/Blueprints/Construction Part Blueprint")]
    internal class ConstructionPartBlueprint : ScriptableObject, IConstructionPartBlueprint
    {
        [SerializeField] protected GameObject Prefab;
        [SerializeField] private byte _initialPool = 5;
        [SerializeField, Space] private float _margin = 0;
        [SerializeField] private float _width = 0;
        [SerializeField] private bool _autoWidth = true;
        [SerializeField, Space] private float _interfereWidth = 0;
        [SerializeField] private bool _autoInterfereWidth = true;
        [SerializeField] private bool _sameWidth = true;
        [SerializeField, Space] private bool _useSpriteRenderer = true;
        [SerializeField] private bool _zOrdering = false;
        private IObjectPool<ConstructionPart> _partPool;
        private IObjectPool<ConstructionBlank> _blankPool;
        private GameObject _poolGameObject;
        private System.IDisposable _disposable;

        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints =>
            System.Array.Empty<IConstructionPartBlueprint>();

        public float InterfereWidth => _interfereWidth + _margin;
        public float Width => _width + _margin;
        public float Margin => _margin;

        private void OnEnable()
        {
            float aWidth = 0;
            if (Prefab != null)
            {
                if (_autoWidth || (!_sameWidth && _autoInterfereWidth))
                    aWidth = _useSpriteRenderer
                        ? Prefab.GetComponent<SpriteRenderer>().bounds.size.x
                        : Prefab.GetComponent<BoxCollider2D>().bounds.size.x;
                if (_autoWidth)
                    _width = aWidth;
                if (_sameWidth)
                    _interfereWidth = _width;
                else if (_autoInterfereWidth)
                    _interfereWidth = aWidth;
            }
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        [Inject]
        public void Construct(ReadOnlyReactiveProperty<FocusedScene> sceneSubscriber)
        {
            _disposable = sceneSubscriber.Subscribe(s =>
            {
                if (s == FocusedScene.Main && _poolGameObject == null)
                {
                    _poolGameObject = new($"{Prefab.name} pool");
                    _partPool = new ObjectPool<ConstructionPart>(
                        Create,
                        actionOnDestroy: Wipeout,
                        defaultCapacity: _initialPool);
                    _blankPool = new ObjectPool<ConstructionBlank>(() => new(this, _partPool, _blankPool));
                }
            });
        }

        public IConstructionBlank MakeBlank(float height, float size)
        {
            ConstructionBlank blank = _blankPool.Get();
            blank.Populate(height, size);
            return blank;
        }

        protected virtual GameObject CreateInstance()
        {
            return Instantiate(Prefab);
        }

        private ConstructionPart Create()
        {
            GameObject instance = CreateInstance();
            ISize sizer;
            IOrder orderer;
            ConstructionPart part;
            instance.transform.parent = _poolGameObject.transform;
            instance.SetActive(false);
            if (_useSpriteRenderer)
            {
                SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
                sizer = new RendererSize(renderer);
                orderer = new SpriteOrder(renderer);
                part = new SizedConstructionPart(this, instance.transform, sizer, _partPool, orderer);
            }
            else if (instance.TryGetComponent(out BoxCollider2D collider))
            {
                sizer = new ColliderSize(collider);
                if (_zOrdering)
                    orderer = new TransformOrder(instance.transform);
                else
                    orderer = new VirtualOrder();
                part = new SizedConstructionPart(this, instance.transform, sizer, _partPool, orderer);
            }
            else
            {
                part = new(this, instance.transform, _partPool);
            }

            return part;
        }

        private static void Wipeout(ConstructionPart part)
        {
            part.Dispose();
        }
    }

    internal class ConstructionPartBlueprint<T, TF> : ConstructionPartBlueprint
        where T : MonoBehaviour where TF : IFactory<Object, T>
    {
        private TF _factory;

        [Inject]
        public virtual void Init(TF factory)
        {
            _factory = factory;
        }

        protected override GameObject CreateInstance()
        {
            return _factory.Create(Prefab).gameObject;
        }
    }
}