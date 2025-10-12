using System.Collections.Generic;
using MR.Core;
using UnityEngine;

namespace MR.Systems.Inventory
{
    [CreateAssetMenu(menuName = "MR/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemDef> items = new();
        private Dictionary<string, ItemDef> map;

        public void OnEnable()
        {
            map = new Dictionary<string, ItemDef>(items.Count);
            foreach (var item in items)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.itemID)) continue;
                map[item.itemID] = item;
            }
        }
        public ItemDef Get(string itemID)
        {
            if (map == null) OnEnable();
            map.TryGetValue(itemID, out var def);
            return def;
        }
        public int GetmaxStack(string itemID)
        {
            var def = Get(itemID);
            return def != null ? def.effectiveMaxStack : int.MaxValue;
        }
        public GameObject GetPrefab(string itemID)
        {
            var def = Get(itemID);
            return def != null ? def.prefab : null;
        }
    }
}