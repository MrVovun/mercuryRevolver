using UnityEngine;

namespace MR.Systems.Inventory
{
    public enum ItemType
    {
        Generic,
        Ammo,
        Consumable,
        Quest
    }
    [CreateAssetMenu(menuName = "MR/Item")]
    public class ItemDef : ScriptableObject
    {
        public string itemID;
        public string displayName;
        public Sprite icon;
        public bool consumable;
        public int maxStack = 999;
        public int size = 1;
    }
}