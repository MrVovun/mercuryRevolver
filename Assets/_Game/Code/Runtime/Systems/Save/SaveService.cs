using System.IO;
using System.Text;
using UnityEngine;
using MR.Systems.Inventory;
using MR.Systems.Cases;

namespace MR.Systems.Save
{
    public class SaveService
    {
        private readonly InventoryService inventory;
        private readonly CaseService cases;
        private readonly string savePath;

        public SaveService(InventoryService inventory, CaseService cases)
        {
            this.inventory = inventory;
            this.cases = cases;
            savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        }
        [System.Serializable]
        private class SaveData
        {
            public System.Collections.Generic.Dictionary<string, int> inventory;
            public string activeCaseID;
        }
        public void Save()
        {
            var data = new SaveData
            {
                inventory = new System.Collections.Generic.Dictionary<string, int>(inventory.Snapshot()),
                activeCaseID = cases.ActiveCase?.caseID
            };
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json, Encoding.UTF8);
        }
        public void Load()
        {
            if (!File.Exists(savePath)) return;

            var json = File.ReadAllText(savePath, Encoding.UTF8);
            var data = JsonUtility.FromJson<SaveData>(json);
            if (data?.inventory != null)
            {
                foreach (var kvp in data.inventory)
                {
                    inventory.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}