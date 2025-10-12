using UnityEngine;

namespace MR.Systems.Cases
{
    [CreateAssetMenu(menuName = "MR/Case/Clue")]
    public class ClueDef : ScriptableObject
    {
        public string clueID;
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
    }
}