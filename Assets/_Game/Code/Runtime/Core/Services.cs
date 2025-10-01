using UnityEngine;


namespace MR.Core
{
    public class Services : MonoBehaviour
    {
        public static Services Instance { get; private set; }

        public Systems.Inventory.ItemDatabase ItemDatabase;

        public Systems.Inventory.InventoryService Inventory { get; private set; }
        public Systems.Cases.CaseService CaseService { get; private set; }
        public Systems.Save.SaveService Save { get; private set; }

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Inventory = new Systems.Inventory.InventoryService(ItemDatabase);
            CaseService = new Systems.Cases.CaseService();
            Save = new Systems.Save.SaveService(Inventory, CaseService);
        }
    }
}
