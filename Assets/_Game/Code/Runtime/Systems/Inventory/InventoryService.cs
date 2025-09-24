using System.Collections.Generic;

namespace MR.Systems.Inventory
{
    public class InventoryService
    {
        private readonly Dictionary<string, int> stacks = new();
        public int Count(string itemID) => stacks.TryGetValue(itemID, out var count) ? count : 0;
        public void Add(string itemID, int amount = 1)
        {
            if (!stacks.ContainsKey(itemID)) stacks[itemID] = 0;
            stacks[itemID] = amount;
        }
        public bool Consume(string itemID, int amount = 1)
        {
            if (Count(itemID) < amount) return false;
            stacks[itemID] -= amount;
            return true;
        }
        public IReadOnlyDictionary<string, int> Snapshot() => stacks;
    }
}