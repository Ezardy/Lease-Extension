using System;
using LeaseExtension.World.Contract;
using R3;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using Zenject;

namespace LeaseExtension.World
{
    [CreateAssetMenu(fileName = "WorldModel", menuName = "Scriptable Objects/World Model")]
    [MovedFrom("Aniki.World")]
    internal class WorldModel : ScriptableObject, IWorldModel, IInitializable, IDisposable
    {
        [SerializeField]
        [FormerlySerializedAs("speed")]
        private SerializableReactiveProperty<float> _speed;

        [SerializeField]
        [FormerlySerializedAs("perspective")]
        private SerializableReactiveProperty<float> _perspective;

        [SerializeField]
        [FormerlySerializedAs("gravity")]
        private SerializableReactiveProperty<float> _gravity;
        private readonly ReactiveProperty<float> _actualSpeed = new(1);
        private float _amplifier = 1;
        private IDisposable _disposable;

        public float Speed => _actualSpeed.CurrentValue;
        public float Perspective => _perspective.CurrentValue;
        public float Gravity => _gravity.CurrentValue;

        public float SpeedAmplifier
        {
            get => _amplifier;
            set
            {
                _amplifier = value;
                _actualSpeed.Value = _speed.CurrentValue * value;
            }
        }

        public Observable<float> SpeedChanged => _actualSpeed;
        public Observable<float> PerspectiveChanged => _perspective;
        public Observable<float> GravityChanged => _gravity;

        public void Initialize()
        {
            _disposable = _speed.Subscribe(s => _actualSpeed.Value = s * _amplifier);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
