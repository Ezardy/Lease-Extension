using System.Runtime.InteropServices;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.WebPlugin.SyncFS
{
    [MovedFrom("Aniki.Save")]
    public static partial class Plugin
    {
        [DllImport("__Internal")]
        public static extern void SyncFS();
    }
}
