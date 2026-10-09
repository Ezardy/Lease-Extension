using System.Collections.Generic;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Concrete;
using R3;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "VirtualConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Virtual Construction Part Blueprint")]
    internal class VirtualConstructionPartBlueprint : ScriptableObject, IConstructionPartBlueprint, IConstructionBlank
    {
        [SerializeField] private byte _initialPool = 5;
        [SerializeField] private float _width = 1;
        private IObjectPool<VirtualConstructionPart> _pool;
        private System.IDisposable _disposable;

        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints => System.Array.Empty<IConstructionPartBlueprint>();
        public float InterfereWidth => _width;
        public float Width => 0;
        public float Margin => 0;
        
        [Inject]
        public void Init(ReadOnlyReactiveProperty<FocusedScene> sceneSubscriber)
        {
            _disposable = sceneSubscriber.Subscribe(s =>
            {
                if (s == FocusedScene.Main && _pool == null)
                {
                    _pool = new ObjectPool<VirtualConstructionPart>(
                        Create,
                        actionOnRelease: Release,
                        defaultCapacity: _initialPool);
                }
            });
        }

        public IConstructionPart Construct(Vector2 position, float scale, int order)
        {
            VirtualConstructionPart part = _pool.Get();
            part.Move(position.x);
            return part;
        }

        public IConstructionBlank MakeBlank(float height, float size)
        {
            return this;
        }

        private VirtualConstructionPart Create()
        {
            return new(this);
        }

        private static void Release(VirtualConstructionPart part)
        {
            part.Move(-part.X);
        }
        
        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
