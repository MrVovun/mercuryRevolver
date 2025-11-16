using UnityEngine;
using MR.Systems.Inventory;
using MR.Systems.Input;
using MR.Systems.Combat;

namespace MR.Systems.Selection
{
    public class WeaponHotbar : MonoBehaviour
    {
        [Header("Weapon Item IDs (max 4)")]
        public string weapon1Id;
        public string weapon2Id;
        public string weapon3Id;
        public string weapon4Id;

        [Header("Equip")]
        public Transform hand;

        private GameObject equippedGO;
        private WeaponBase equippedWeapon;

        public WeaponBase EquippedWeapon => equippedWeapon;
        public GameObject EquippedRoot => equippedGO;

        void Update()
        {
            if (InputR.Weapon1) EquipSlot(1);
            if (InputR.Weapon2) EquipSlot(2);
            if (InputR.Weapon3) EquipSlot(3);
            if (InputR.Weapon4) EquipSlot(4);

            if (equippedWeapon != null && InputR.Fire) equippedWeapon.Fire();
        }

        public void EquipSlot(int idx)
        {
            string id = idx switch { 1 => weapon1Id, 2 => weapon2Id, 3 => weapon3Id, 4 => weapon4Id, _ => null };
            if (string.IsNullOrWhiteSpace(id)) return;

            var def = MR.Core.Services.Instance?.ItemDatabase?.Get(id);
            if (def == null || def.prefab == null) return;

            Clear();
            equippedGO = Instantiate(def.prefab, hand);
            equippedGO.transform.localPosition = Vector3.zero;
            equippedGO.transform.localRotation = Quaternion.identity;
            equippedWeapon = equippedGO.GetComponentInChildren<WeaponBase>();
        }

        private void Clear()
        {
            if (equippedGO) Destroy(equippedGO);
            equippedGO = null; equippedWeapon = null;
        }
    }
}
