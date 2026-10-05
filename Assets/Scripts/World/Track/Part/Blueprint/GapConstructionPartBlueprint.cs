using System;
using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.SceneManagment.Contract.Message;
using LeaseExtension.World.Contract;
using LeaseExtension.World.Track.Part.Blank;
using LeaseExtension.World.Track.Part.Concrete;
using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    [CreateAssetMenu(fileName = "GapConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Gap Construction Part Blueprint")]
    internal class GapConstructionPartBlueprint : ScriptableObject, IGapConstructionPartBlueprint
    {
        [SerializeField, Range(0, 1)]
        private float _gapSize;
        [SerializeField]
        private Ref<IConstructionPartBlueprint> _bottomPart;
        [SerializeField]
        private Ref<IConstructionPartBlueprint> _gapPart;
        [SerializeField]
        private Ref<IConstructionPartBlueprint> _topPart;
        private IObjectPool<GapConstructionPart> _partPool;
        private IObjectPool<GapConstructionBlank> _blankPool;
        private IDisposable _disposable;

        public IReadOnlyCollection<IConstructionPartBlueprint> SubPartBlueprints { get; private set; }
        public float Width { get; private set; }
        public float Margin { get; private set; }
        public float GapSize => _gapSize;
        public float InterfereWidth { get; private set; }

        private void OnEnable()
        {
            if (_bottomPart && _gapPart && _topPart)
            {
                Width = Mathf.Max(_bottomPart.I.Width, _gapPart.I.Width, _topPart.I.Width);
                Margin = Mathf.Max(_bottomPart.I.Margin, _gapPart.I.Margin, _topPart.I.Margin);
                InterfereWidth = Mathf.Max(
                    _bottomPart.I.InterfereWidth,
                    _gapPart.I.InterfereWidth,
                    _topPart.I.InterfereWidth);
                SubPartBlueprints = new[]
                {
                    _bottomPart.I,
                    _gapPart.I,
                    _topPart.I
                };
            }
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public IConstructionBlank MakeBlank(float height, float gapPos)
        {
            float gapStart = gapPos - _gapSize / 2 + height;
            GapConstructionBlank blank = _blankPool.Get();
            blank.Populate(_bottomPart.I.MakeBlank(height, gapStart), _gapPart.I.MakeBlank(gapStart, _gapSize), _topPart.I.MakeBlank(
                gapStart + _gapSize,
                1 - gapStart - _gapSize));
            return blank;
        }

        [Inject]
        public void Construct(ISubscriber<FocusedScene> sceneSubscriber)
        {
            _disposable = sceneSubscriber.Subscribe(_ =>
            {
                if (_blankPool == null)
                {
                    _blankPool = new ObjectPool<GapConstructionBlank>(
                        () => new(this, _blankPool, _partPool),
                        actionOnRelease: b => b.Depopulate());
                    _partPool = new ObjectPool<GapConstructionPart>(
                        () => new(this, _partPool),
                        actionOnRelease: p => p.Depopulate());
                }
            }, FocusedSceneFilter.Main);
        }
    }
}
