using System.Collections.Generic;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;
using LeaseExtension.World.Track.Part.Order;
using LeaseExtension.World.Track.Part.Size;
using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "ConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Construction Part Blueprint")]
    internal class ConstructionPartBlueprint : ScriptableObject, IConstructionPartBlueprint
    {
        [SerializeField]
        [FormerlySerializedAs("prefab")]
        protected GameObject Prefab;

        [SerializeField]
        [FormerlySerializedAs("initialPool")]
        private byte _initialPool = 5;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("margin")]
        private float _margin = 0;

        [SerializeField]
        [FormerlySerializedAs("width")]
        private float _width = 0;

        [SerializeField]
        [FormerlySerializedAs("autoWidth")]
        private bool _autoWidth = true;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("interfereWidth")]
        private float _interfereWidth = 0;

        [SerializeField]
        [FormerlySerializedAs("autoInterfereWidth")]
        private bool _autoInterfereWidth = true;

        [SerializeField]
        [FormerlySerializedAs("sameWidth")]
        private bool _sameWidth = true;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("useSpriteRenderer")]
        private bool _useSpriteRenderer = true;

        [SerializeField]
        [FormerlySerializedAs("zOrdering")]
        private bool _zOrdering = false;
        private IObjectPool<ConstructionPart> _partPool;
        private IObjectPool<ConstructionBlank> _blankPool;
        private GameObject _poolGameObject;
        private System.IDisposable _disposable;

        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints => System.Array.Empty<IConstructionPartBlueprint>();
        public float InterfereWidth => _interfereWidth + _margin;
        public float Width => _width + _margin;
        public float Margin => _margin;

        private void OnEnable()
        {
            float aWidth = 0;
            if (Prefab != null)
            {
                if (_autoWidth || (!_sameWidth && _autoInterfereWidth))
                    aWidth = _useSpriteRenderer ? Prefab.GetComponent<SpriteRenderer>().bounds.size.x : Prefab.GetComponent<BoxCollider2D>().bounds.size.x;
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
        public void Construct(ISubscriber<FocusedScene> sceneSubscriber)
        {
            _disposable = sceneSubscriber.Subscribe(_ =>
            {
                if (_poolGameObject == null)
                {
                    _poolGameObject = new($"{Prefab.name} pool");
                    _partPool = new ObjectPool<ConstructionPart>(
                        Create,
                        actionOnDestroy: Wipeout,
                        defaultCapacity: _initialPool);
                    _blankPool = new ObjectPool<ConstructionBlank>(() => new(this, _partPool, _blankPool));
                }
            }, FocusedSceneFilter.Main);
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

    [MovedFrom("Aniki.World")]
    internal class ConstructionPartBlueprint<T, F> : ConstructionPartBlueprint where T : MonoBehaviour where F : IFactory<Object, T>
    {
        private F _factory;

        [Inject]
        public virtual void Init(F factory)
        {
            _factory = factory;
        }

        protected override GameObject CreateInstance()
        {
            return _factory.Create(Prefab).gameObject;
        }
    }
}
