#if UNITY_EDITOR
using UnityEditor;

namespace MR.EditorTools
{
    public static class QuickMenu
    {
        [MenuItem("Tools/MR/Bootstrap: Clear Remembered Scene")]
        public static void ClearRemembered() => MR.Core.BootstrapLoader.ClearRemembered();

        [MenuItem("Tools/MR/Open Build Settings")]
        public static void OpenBuildSettings() => EditorWindow.GetWindow(typeof(BuildPlayerWindow));
    }
}
#endif