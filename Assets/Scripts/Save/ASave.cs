using System;
using System.IO;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using LeaseExtension.WebPlugin.SyncFS;
#endif

namespace LeaseExtension.Save
{
    public abstract class ASave<TData> : ScriptableObject, ISave where TData : struct
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        private const string PERSISTENT_DATA_PATH =
            "/idbfs/3dcd2e7bcb436cf4f1ac81d0c8ddf86a"; // MD5 hashed "Lease_Extension_1.0"
#else
        private static string _persistentDataPath;
#endif

        [SerializeField] protected TData SaveData;
        [SerializeField] private bool _doLoad = true;
        [SerializeField] private string _filename;

        private string _path;
        public TData Data => SaveData;
        private static string PersistentDataPath
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            get => PERSISTENT_DATA_PATH;
#else
            get => _persistentDataPath;
#endif
        }

        private void OnEnable()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            _persistentDataPath = Application.persistentDataPath;
#endif
            _path = Path.Join(PersistentDataPath, _filename);
        }

        protected void Init()
        {
            if (!Directory.Exists(PersistentDataPath))
                Directory.CreateDirectory(PersistentDataPath);
            if (!File.Exists(_path))
                Save();
        }

        public void Load()
        {
            if (_doLoad)
                try
                {
                    SaveData = JsonUtility.FromJson<TData>(File.ReadAllText(_path));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
        }

        public void Save()
        {
            File.WriteAllText(_path, JsonUtility.ToJson(SaveData));
#if UNITY_WEBGL && !UNITY_EDITOR
            Plugin.SyncFS();
#endif
        }
    }
}
