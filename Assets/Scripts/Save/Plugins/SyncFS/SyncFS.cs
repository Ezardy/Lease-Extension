using System.Runtime.InteropServices;

namespace Aniki.Save {
	public static partial class Plugin {
		[DllImport("__Internal")]
		public static extern void	SyncFS();
	}
}
