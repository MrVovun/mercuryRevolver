using UnityEngine;

namespace MR.Systems.Combat
{
    [CreateAssetMenu(menuName = "MR/Combat/Weapon Stats")]
    public class WeaponStats : ScriptableObject
    {
        public string weaponId = "weapon_id";
        public string ammoId = "ammo_id";

        public float damage = 10f;
        public float staggerMultiplier = 1f;
        public float staggerThreshold = 10f;

        public float range = 50f;
        public float spreadDegrees = 1f;
        public int pellets = 1;
        public float pelletsSpreadDegrees = 4f;

        public float roundsPerMinute = 450f;
        public int magazineSize = 12;
        public float reloadTime = 1.5f;
        public float perShellReloadTime = 0.5f; // for shotguns
        public float pumpTime = 0.5f; // for shotguns

        public float noise = 1.5f;
        public float recoilKick = 0.04f;
    }
}