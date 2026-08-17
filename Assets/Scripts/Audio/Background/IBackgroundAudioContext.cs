using Aniki.State;
using UnityEngine;

namespace Aniki.Audio {
	internal interface IBackgroundAudioContext : IContext {
		public Animator	Animator { get; }
	}
}
