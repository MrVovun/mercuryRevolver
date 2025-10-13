using UnityEngine;
using MR.Core;

namespace MR.Systems.Combat.Weapons
{
    public class PumpShotgun : WeaponBase
    {
        [Header("Ejection")]
        public Transform ejectionPort;
        public GameObject shellPrefab;
        public Vector3 shellEjectVelocity = new(0.3f, 1.1f, -0.2f);
        public Vector3 shellAngular = new(0f, 10f, 120f);

        [Header("Damage Falloff")]
        public AnimationCurve damageFalloff = AnimationCurve.Linear(0,1,1,0.5f);

        private int inTube;
        private bool cycling;
        private bool loading;

        protected override void Awake()
        {
            base.Awake();
            if (!stats) { Debug.LogError("PumpShotgun missing WeaponStats"); return; }
            inTube = Mathf.Clamp(inTube, 0, stats.magazineSize);
            if (stats.staggerMultiplier < 1.5f) stats.staggerMultiplier = 2.2f;
        }

        public override void Fire()
        {
            if (!stats || cycling || loading) return;

            if (inTube <= 0)
            {
                TryLoadShell(); // quick top-up attempt
                return;
            }

            inTube--;
            FirePellets();

            PlayMuzzleAndSfx(sfxFire);
            PingAttention();
            KickRecoil();

            StartCoroutine(PumpCycle());
        }

        private void FirePellets()
        {
            int pellets = Mathf.Max(1, stats.pellets);
            float spread = Mathf.Max(stats.pelletsSpreadDegrees, stats.spreadDegrees);

            var origin = shootCam ? shootCam.transform.position : transform.position;
            for (int i = 0; i < pellets; i++)
            {
                var dir = GetSpreadDirection(spread);
                if (Physics.Raycast(origin, dir, out var hit, stats.range, hitMask, QueryTriggerInteraction.Ignore))
                {
                    float dist01 = Mathf.Clamp01(hit.distance / Mathf.Max(0.01f, stats.range));
                    float pelletDmg = stats.damage * damageFalloff.Evaluate(dist01);
                    ApplyManifestDamage(hit.collider.gameObject, pelletDmg);
                }
            }
        }

        private System.Collections.IEnumerator PumpCycle()
        {
            cycling = true;
            // Eject spent shell at pump start
            SpawnEjection(shellPrefab, ejectionPort ? ejectionPort : muzzle, shellEjectVelocity, shellAngular);

            if (audioSrc && sfxPumpOrCycle) audioSrc.PlayOneShot(sfxPumpOrCycle);
            yield return new WaitForSeconds(stats.pumpTime);
            cycling = false;
        }

        public void StartReloadHold()
        {
            if (!loading) StartCoroutine(LoadShellsLoop());
        }

        public void StopReloadHold() { loading = false; }

        private System.Collections.IEnumerator LoadShellsLoop()
        {
            loading = true;
            while (loading && inTube < stats.magazineSize)
            {
                if (!TryLoadShell()) break;
                if (audioSrc && sfxReload) audioSrc.PlayOneShot(sfxReload);
                yield return new WaitForSeconds(stats.perShellReloadTime);
            }
            loading = false;
        }

        private bool TryLoadShell()
        {
            if (!stats) return false;
            if (inTube >= stats.magazineSize) return false;
            if (!Services.Instance.Inventory.Consume(stats.ammoId, 1)) return false;
            inTube++;
            return true;
        }

        public (int inTube, int reserve) GetAmmoCounts()
        {
            int reserve = Services.Instance.Inventory.Count(stats ? stats.ammoId : "");
            return (inTube, reserve);
        }
    }
}