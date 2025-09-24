using UnityEngine;

namespace MR.Systems.Cases
{
    [CreateAssetMenu(menuName = "MR/Case/ManifestArchetype")]
    public class ManifestArchetype : ScriptableObject
    {
        public string manifestID;
        public string displayName;
        [TextArea] public string description;
        public float baseSpeed = 2.2f;
        public float engageSpeed = 3.4f;
        public float staggerSeconds = 1.0f;
    }
}