using UnityEngine;

namespace Aniki.Common {
	[System.Serializable]
	public class IRef<T> : ISerializationCallbackReceiver where T : class {
		[SerializeField] private Object	target;
		public T	I { get => target as T; }

		public static implicit operator bool(IRef<T> ir) => ir.target != null;

		private void	OnValidate() {
			if (target is not T) {
				if (target is GameObject go) {
					target = null;
					foreach (Component c in go.GetComponents<Component>()) {
						if (c is T) {
							target = c;
							break;
						}
					}
				} else
					target = null;
			}
		}
	
		void ISerializationCallbackReceiver.OnBeforeSerialize() => OnValidate();
		void ISerializationCallbackReceiver.OnAfterDeserialize() { }
	}
}
