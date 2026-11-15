using System.Runtime.InteropServices;

namespace LeaseExtension.WebPlugin.SyncFS
{
    public static partial class Plugin
    {
        [DllImport("__Internal")]
        public static extern void SyncFS();
    }
}
