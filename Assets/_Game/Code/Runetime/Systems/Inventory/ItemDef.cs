using UnityEngine;

namespace MR.Systems.Inventory
{
    [CreateAssetMenu(menuName = "MR/Item")]
    public class ItemDef : ScriptableObject
    {
        public string itemID;
        public string displayName;
        public Sprite icon;
        public bool consumable;
        public int maxStack = 99;
        public int size = 1;
    }
}