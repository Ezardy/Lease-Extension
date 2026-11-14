using System;
using System.IO;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using LeaseExtension.WebPlugin.SyncFS;
#endif

namespace LeaseExtension.Save
{
    public abstract class SaveBase<TData> : ScriptableObject, ISave where TData : struct
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        private const string PERSISTENT_DATA_PATH =
            "/idbfs/9ce8c2e97e65868c85543b62f516aec1"; // MD5 hashed "Lease_Extension_1.1"
#else
        private static string _persistentDataPath;
#endif

        [SerializeField] protected TData SaveData;
        [SerializeField] private string _filename;
#if UNITY_EDITOR        
        [SerializeField] private bool _doLoad = true;
        [SerializeField] private bool _doSave = true;
#endif        

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
            {
                Directory.CreateDirectory(PersistentDataPath);
            }

            if (!File.Exists(_path))
            {
                Save();
            }
        }

        public void Load()
        {
#if UNITY_EDITOR
            if (!_doLoad)
            {
                return;
            }
#endif            
            if (File.Exists(_path))
            {
                try
                {
                    SaveData = JsonUtility.FromJson<TData>(File.ReadAllText(_path));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void Save()
        {
#if UNITY_EDITOR
            if (!_doSave)
            {
                return;
            }
#endif            
            File.WriteAllText(_path, JsonUtility.ToJson(SaveData));
#if UNITY_WEBGL && !UNITY_EDITOR
            Plugin.SyncFS();
#endif
        }
    }
}