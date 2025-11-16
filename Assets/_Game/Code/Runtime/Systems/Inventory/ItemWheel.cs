using UnityEngine;
using System.Collections.Generic;
using MR.Systems.Inventory;
using MR.Systems.Input;
using MR.Systems.Tools;
using MR.Systems.Use;

namespace MR.Systems.Selection
{
    public class ItemWheel : MonoBehaviour
    {
        [Header("Eligible Items (IDs)")]
        public List<string> itemIds = new(); // tools/consumables only

        [Header("Equip")]
        public Transform hand;
        public float timescaleWhileOpen = 0.1f;
        public float deadzone = 0.25f; // stick deadzone for selection

        private GameObject equippedGO;
        private ItemDef equippedDef;
        private ToolBase equippedTool;
        private ConsumableUse equippedConsumable;

        private bool open;
        private int hoverIndex;

        void Update()
        {
            if (InputR.ItemWheel)
            {
                if (!open) Open();
                UpdateHover();
            }
            else
            {
                if (open) Close(select:true);
            }

            if (InputR.UseItem)
            {
                if (equippedConsumable) equippedConsumable.UseOnce();
                if (equippedTool && equippedTool.IsReady) equippedTool.TryUse();
            }

            if (equippedTool) equippedTool.Tick();
        }

        private void Open()
        {
            open = true;
            Time.timeScale = timescaleWhileOpen;
            hoverIndex = Mathf.Clamp(hoverIndex, 0, Mathf.Max(0, itemIds.Count - 1));
        }

        private void Close(bool select)
        {
            open = false;
            Time.timeScale = 1f;
            if (select && itemIds.Count > 0) EquipIndex(hoverIndex);
        }

        private void UpdateHover()
        {
            if (itemIds.Count == 0) return;

            // Use mouse position if present; otherwise use Look stick direction.
            Vector2 dir = Vector2.zero;
#if ENABLE_INPUT_SYSTEM
            if (UnityEngine.InputSystem.Mouse.current != null)
            {
                var screen = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                var center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                dir = (screen - center).normalized;
            }
            else
#endif
            {
                dir = InputR.Look.normalized;
            }

            if (dir.magnitude < deadzone) return;

            float angle = Mathf.Atan2(dir.y, dir.x); // -pi..pi
            if (angle < 0) angle += Mathf.PI * 2f;    // 0..2pi

            float slice = (Mathf.PI * 2f) / itemIds.Count;
            hoverIndex = Mathf.Clamp(Mathf.FloorToInt(angle / slice), 0, itemIds.Count - 1);
        }

        private void EquipIndex(int i)
        {
            ClearEquipped();

            var id = itemIds[Mathf.Clamp(i, 0, itemIds.Count - 1)];
            var def = MR.Core.Services.Instance?.ItemDatabase?.Get(id);
            if (def == null || def.prefab == null) return;

            equippedDef = def;
            equippedGO = Instantiate(def.prefab, hand);
            equippedGO.transform.localPosition = Vector3.zero;
            equippedGO.transform.localRotation = Quaternion.identity;

            equippedTool = equippedGO.GetComponentInChildren<ToolBase>();
            equippedConsumable = equippedGO.GetComponentInChildren<ConsumableUse>();
        }

        private void ClearEquipped()
        {
            if (equippedGO) Destroy(equippedGO);
            equippedGO = null; equippedDef = null; equippedTool = null; equippedConsumable = null;
        }
    }
}
