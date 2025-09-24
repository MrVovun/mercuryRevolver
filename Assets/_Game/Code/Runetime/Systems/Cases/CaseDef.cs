using UnityEngine;

namespace MR.Systems.Cases
{
    [CreateAssetMenu(menuName = "MR/Case/Case")]
    public class CaseDef : ScriptableObject
    {
        public string caseID;
        public string displayName;
        [TextArea] public string description;
        public ManifestArchetype manifest;
        public ClueDef[] clues;
        public string sceneName;
    }
}