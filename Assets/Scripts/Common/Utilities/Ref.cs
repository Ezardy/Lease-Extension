using UnityEngine;

namespace LeaseExtension.Common.Utilities
{
    [System.Serializable]
    public class Ref<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField] private Object _target;

        public T I => _target as T;

        public static implicit operator bool (Ref<T> ir)
        {
            return ir._target != null;
        }

        private void OnValidate()
        {
            if (_target is not T)
            {
                if (_target is GameObject go)
                {
                    _target = null;
                    foreach (Component c in go.GetComponents<Component>())
                        if (c is T)
                        {
                            _target = c;
                            break;
                        }
                }
                else
                {
                    _target = null;
                }
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            OnValidate();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }
    }
}
