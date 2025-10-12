using UnityEngine;
using System.Collections.Generic;
using MR.Systems.Inventory;
using MR.Systems.Input;
using MR.Systems.Tools;
using MR.Systems.Combat;
using MR.Systems.Use;

namespace MR.Systems.Selection
{
    public class ItemWheel : MonoBehaviour
    {
        [Header("Hotbar (Item IDs)")]
        public List<string> hotbarItemIds = new(); // assign in Inspector

        [Header("Equip")]
        public Transform hand; // where items are spawned/equipped

        private int index;
        private GameObject equippedGO;
        private ItemDef equippedDef;
        private ToolBase equippedTool;
        private WeaponBase equippedWeapon;
        private ConsumableUse equippedConsumable;

        void Start() { EquipIndex(0); }

        void Update()
        {
            if (InputR.NextTool) Cycle();

            // Route primary action
            if (equippedWeapon != null && InputR.Fire) equippedWeapon.Attack();
            if (equippedTool != null)
            {
                if (InputR.Aim && equippedTool.IsReady) equippedTool.TryUse();
                equippedTool.Tick();
            }
            if (equippedConsumable != null && InputR.Fire)
                equippedConsumable.UseOnce();
        }

        private void Cycle()
        {
            index = (index + 1) % Mathf.Max(1, hotbarItemIds.Count);
            EquipIndex(index);
        }

        private void EquipIndex(int i)
        {
            ClearEquipped();

            if (hotbarItemIds.Count == 0) return;

            var id = hotbarItemIds[Mathf.Clamp(i, 0, hotbarItemIds.Count - 1)];
            var def = MR.Core.Services.Instance?.ItemDatabase?.Get(id);
            if (def == null) return;

            equippedDef = def;
            if (def.prefab != null && hand != null)
            {
                equippedGO = Instantiate(def.prefab, hand);
                equippedGO.transform.localPosition = Vector3.zero;
                equippedGO.transform.localRotation = Quaternion.identity;

                // Cache behaviors (if present)
                equippedTool = equippedGO.GetComponentInChildren<ToolBase>();
                equippedWeapon = equippedGO.GetComponentInChildren<WeaponBase>();
                equippedConsumable = equippedGO.GetComponentInChildren<ConsumableUse>();
            }
        }

        private void ClearEquipped()
        {
            if (equippedGO != null) Destroy(equippedGO);
            equippedGO = null;
            equippedDef = null;
            equippedTool = null;
            equippedWeapon = null;
            equippedConsumable = null;
        }
    }
}