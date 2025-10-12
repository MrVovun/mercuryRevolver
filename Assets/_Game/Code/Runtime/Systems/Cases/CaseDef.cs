using UnityEditor;
using UnityEngine;

namespace MR.Systems.Cases
{
    [CreateAssetMenu(menuName = "MR/Case/Case")]
    public class CaseDef : ScriptableObject
    {
        public string caseID;
        public string title;
        [TextArea] public string description;
        public ManifestArchetype manifest;
        public ClueDef[] requiredClues;

#if UNITY_EDITOR
        [Header("Scene (Editor)")]
        public SceneAsset caseScene;
#endif
        [Header("Scene (Runtime)")]
        public string caseSceneName;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (caseScene != null)
            {
                var path = AssetDatabase.GetAssetPath(caseScene);
                var name = System.IO.Path.GetFileNameWithoutExtension(path);
                if (caseSceneName != name)
                {
                    caseSceneName = name;
                }
            }
        }
    }
#endif
}