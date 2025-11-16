#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;

namespace MR.EditorTools
{
    [InitializeOnLoad]
    public class LastPlayedSceneRecorder
    {
        static LastPlayedSceneRecorder()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.ExitingPlayMode)
            {
                var activeScene = SceneManager.GetActiveScene();
                if (activeScene.IsValid() && !string.IsNullOrEmpty(activeScene.name))
                {
                    MR.Core.BootstrapLoader.RememberLastPlayed(activeScene.name);
                }
            }
        }
    }
}
#endif