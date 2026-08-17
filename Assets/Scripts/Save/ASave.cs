using System.IO;
using UnityEngine;

namespace Aniki.Save {
	public abstract class ASave<TData, TModel> : ScriptableObject, ISave where TData : struct {
		
		#if UNITY_WEBGL && !UNITY_EDITOR
			private const string	persistentDataPath = "/idbfs/3dcd2e7bcb436cf4f1ac81d0c8ddf86a"; // MD5 hashed "Lease_Extension_1.0"
		#else
			private static string	persistentDataPath;
		#endif

		[SerializeField] private bool		doLoad = true;
		[SerializeField] private string		filename;
		[SerializeField] protected TData	data;

		public string	Path => path;
		public TData	Data => data;

		private string	path;

		public void	Init() {
			if (!Directory.Exists(persistentDataPath))
				Directory.CreateDirectory(persistentDataPath);
			if (!File.Exists(path))
				Save();
		}

		public void	Load() {
			if (doLoad) {
				try {
					data = JsonUtility.FromJson<TData>(File.ReadAllText(path));
				} catch (IOException) { }
			}
		}
		public void	Save() {
			File.WriteAllText(path, JsonUtility.ToJson(data));
			#if UNITY_WEBGL && !UNITY_EDITOR
			Plugin.SyncFS();
			#endif
		}

		public void	OnEnable() {
			#if !UNITY_WEBGL || UNITY_EDITOR
			persistentDataPath = Application.persistentDataPath;
			#endif
			path = System.IO.Path.Join(persistentDataPath, filename);
		}
	}
}
