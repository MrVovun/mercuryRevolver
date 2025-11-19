using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR.Core
{
    public class BootstrapLoader : MonoBehaviour
    {
        [Header("Loading")]
        [Tooltip("Default scene to load after Bootstrap.")]
        public string defaultSceneName;

        [Tooltip("In the Unity Editor, load the last played scene instead of default hub.")]
        public bool loadLastPlayedInEditor = true;

        [Tooltip("Delay 1 frame before loading to ensure singletons are initialized.")]
        public bool yieldOneFrame = true;

#if UNITY_EDITOR
        private const string EditorPrefKey = "MR_LastPlayedSceneName";
#endif

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            if (yieldOneFrame)
            {
                StartCoroutine(LoadRoutine());
            }
            else
            {
                LoadNext();
            }
        }
        System.Collections.IEnumerator LoadRoutine()
        {
            yield return null;
            LoadNext();
        }
        private void LoadNext()
        {
#if UNITY_EDITOR
            if (loadLastPlayedInEditor)
            {
                var last = UnityEditor.EditorPrefs.GetString(EditorPrefKey, "");
                if (!string.IsNullOrEmpty(last))
                {
                    SafeLoad(last);
                    return;
                }
            }
#endif
            SafeLoad(defaultSceneName);
        }
        private void SafeLoad(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("[BootstrapLoader] Target scene name is empty. Aborting.");
                return;
            }
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError($"[BootstrapLoader] Scene '{sceneName}' is not in Build Settings. Aborting.");
                return;
            }
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
#if UNITY_EDITOR
        public static void RememberLastPlayed(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                UnityEditor.EditorPrefs.SetString(EditorPrefKey, sceneName);
            }
        }
        public static void ClearRemembered()
        {
            UnityEditor.EditorPrefs.DeleteKey(EditorPrefKey);
        }
#endif
    }
}