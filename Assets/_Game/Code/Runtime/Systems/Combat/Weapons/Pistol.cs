using UnityEngine;
using MR.Core;

namespace MR.Systems.Combat.Weapons
{
    public class Pistol : WeaponBase
    {
        [Header("Ejection")]
        public Transform ejectionPort;
        public GameObject casingPrefab; // small brass + EjectableNoise
        public Vector3 casingEjectVelocity = new(0.6f, 1.2f, -0.2f);
        public Vector3 casingAngular = new(0f, 20f, 90f);

        public GameObject magazinePrefab; // dropped mag + EjectableNoise
        public Vector3 magEjectVelocity = new(0.1f, 0.6f, -0.1f);
        public Vector3 magAngular = new(0f, 7f, 20f);

        private int inMag;
        private float nextFireAt;
        private bool reloading;

        protected override void Awake()
        {
            base.Awake();
            if (!stats) { Debug.LogError("RaycastPistol missing WeaponStats"); return; }
            inMag = Mathf.Clamp(inMag, 0, stats.magazineSize);
        }

        public override void Fire()
        {
            if (!stats || reloading) return;
            if (Time.time < nextFireAt) return;

            if (inMag <= 0)
            {
                if (audioSrc && sfxDry) audioSrc.PlayOneShot(sfxDry);
                nextFireAt = Time.time + 60f / stats.roundsPerMinute;
                return;
            }

            inMag--;
            ShootOnce();
            // Eject spent casing immediately
            SpawnEjection(casingPrefab, ejectionPort, casingEjectVelocity, casingAngular);

            PlayMuzzleAndSfx(sfxFire);
            PingAttention();
            KickRecoil();

            nextFireAt = Time.time + 60f / stats.roundsPerMinute;
        }

        private void ShootOnce()
        {
            var origin = shootCam ? shootCam.transform.position : transform.position;
            var dir = GetSpreadDirection(stats.spreadDegrees);

            if (Physics.Raycast(origin, dir, out var hit, stats.range, hitMask, QueryTriggerInteraction.Ignore))
                ApplyManifestDamage(hit.collider.gameObject);
        }

        /// Mag-swap: requires full magazine in reserve. If not enough ammo, reload fails (feedback only).
        public void Reload()
        {
            if (reloading || !stats) return;
            if (Services.Instance.Inventory.Count(stats.ammoId) < stats.magazineSize) // must have a full mag
            {
                // play dry reload sound
                if (audioSrc && sfxDry) audioSrc.PlayOneShot(sfxDry);
                return;
            }

            reloading = true;

            // Eject the current magazine immediately (remaining bullets are discarded)
            SpawnEjection(magazinePrefab, ejectionPort ? ejectionPort : muzzle, magEjectVelocity, magAngular);

            if (audioSrc && sfxReload) audioSrc.PlayOneShot(sfxReload);
            StartCoroutine(ReloadRoutine());
        }

        private System.Collections.IEnumerator ReloadRoutine()
        {
            yield return new WaitForSeconds(stats.reloadTime);

            // Consume a full magazine now (after time passes to avoid dupes across save/load)
            int loaded = 0;
            for (int i = 0; i < stats.magazineSize; i++)
            {
                if (!Services.Instance.Inventory.Consume(stats.ammoId, 1)) break;
                loaded++;
            }
            if (loaded == stats.magazineSize)
            {
                inMag = stats.magazineSize;
            }
            else
            {
                // Edge case: inventory changed mid-reload (e.g., cheated away). Keep previous inMag.
                if (audioSrc && sfxDry) audioSrc.PlayOneShot(sfxDry);
            }

            reloading = false;
        }

        public (int inMag, int reserve) GetAmmoCounts()
        {
            int reserve = Services.Instance.Inventory.Count(stats ? stats.ammoId : "");
            return (inMag, reserve);
        }
    }
}