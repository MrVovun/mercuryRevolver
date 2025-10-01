using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MR.Systems.Inventory
{
    public class InventoryService
    {
        private readonly Dictionary<string, int> stacks = new();
        private readonly ItemDatabase db;

        public InventoryService(ItemDatabase itemDatabase)
        {
            db = itemDatabase;
        }

        // Current count for a given itemId.
        public int Count(string itemID) => stacks.TryGetValue(itemID, out var count) ? count : 0;
        
        // Tries to add amount to the stack, respecting ItemDef.maxStack.
        // Returns how many were actually added.
        public int TryAdd(string itemID, int amount)
        {
            if (amount <= 0) return 0;

            var max = db != null ? db.getmaxStack(itemID) : int.MaxValue;
            var current = Count(itemID);
            var toAdd = Math.Clamp(amount, 0, Math.Max(0, max - current));

            if (toAdd <= 0) return 0;

            stacks[itemID] = current + toAdd;
            return toAdd;
        }
        public void Add(string itemID, int amount = 1) => TryAdd(itemID, amount);

        public bool Consume(string itemID, int amount = 1)
        {
            if (amount <= 0) return false;
            var current = Count(itemID);
            if (current < amount) return false;
            stacks[itemID] = current - amount;
            return true;
        }
        public IReadOnlyDictionary<string, int> Snapshot() => stacks;
    }
}