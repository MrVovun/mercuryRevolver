using System;
using NUnit.Framework;
using UnityEngine;

namespace MR.Systems.Inventory
{
    public enum ItemType
    {
        Generic,
        Ammo,
        Consumable,
        Quest,
        Equipment,
        Currency
    }

    [Flags]
    public enum ItemTags
    {
        None = 0,
        Consumable = 1 << 0, // removed on use
        Stackable = 1 << 1, // >1 quantity makes sense
        Tradable = 1 << 2, // appears in shop
        Droppable = 1 << 3, // can be dropped
        Unique = 1 << 4, // cap stack at 1 regardless of maxStack
        BoundQuest = 1 << 5, // quest-bound; may restrict selling/dropping
        Perishable = 1 << 6, // can expire
    }
    [CreateAssetMenu(menuName = "MR/Item")]
    public class ItemDef : ScriptableObject
    {
        [Tooltip("Unique key, e.g., 'ammo_9mm' or 'keycard_dockA'.")]
        public string itemID;
        public string displayName;
        public Sprite icon;

        [Header("Classification")]
        public ItemType itemType = ItemType.Generic;
        public ItemTags itemTags = ItemTags.None;

        [Header("Stacking & Size")]
        public int maxStack = 999;
        public int size = 1;

        //Helpers
        public bool HasTag(ItemTags tag) => (itemTags & tag) != 0;
        public bool IsStackable() => HasTag(ItemTags.Stackable) && !HasTag(ItemTags.Unique);
        public bool IsConsumable() => HasTag(ItemTags.Consumable);
        public int effectiveMaxStack => HasTag(ItemTags.Unique) ? 1 : Math.Max(1, maxStack);
    }
}