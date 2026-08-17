using R3;
using System;
using UnityEngine;
using Zenject;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "WorldModel", menuName = "Scriptable Objects/World Model")]
	internal class WorldModel : ScriptableObject, IWorldModel, IInitializable, IDisposable {
		[SerializeField] private SerializableReactiveProperty<float>	speed;
		[SerializeField] private SerializableReactiveProperty<float>	perspective;
		[SerializeField] private SerializableReactiveProperty<float>	gravity;

		private readonly ReactiveProperty<float>	actualSpeed = new(1);

		private float	amplifier = 1;

		private IDisposable	disposable;

		public float	Speed => actualSpeed.CurrentValue;
		public float	Perspective => perspective.CurrentValue;
		public float	Gravity => gravity.CurrentValue;

		public float	SpeedAmplifier {
			get => amplifier;
			set {
				amplifier = value;
				actualSpeed.Value = speed.CurrentValue * value;
			}
		}

		public Observable<float>	SpeedChanged => actualSpeed;
		public Observable<float>	PerspectiveChanged => perspective;
		public Observable<float>	GravityChanged => gravity;

		public void	Initialize() {
			disposable = speed.Subscribe(s => actualSpeed.Value = s * amplifier);
		}

		public void	Dispose() {
			disposable.Dispose();
		}
	}
}
