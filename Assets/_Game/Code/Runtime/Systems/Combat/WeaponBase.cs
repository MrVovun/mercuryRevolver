using UnityEngine;
using MR.Systems.AI;
using MR.Core;

namespace MR.Systems.Combat
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Stats")]
        public WeaponStats stats;

        [Header("Refs")]
        public Camera shootCam;
        public Transform muzzle;
        public LayerMask hitMask = ~0;
        public ParticleSystem muzzleFx;
        public AudioSource audioSrc;
        public AudioClip sfxFire;
        public AudioClip sfxDry;
        public AudioClip sfxReload;
        public AudioClip sfxPumpOrCycle;

        protected virtual void Awake()
        {
            if (shootCam == null) shootCam = Camera.main;
        }

        public abstract void Fire();

        protected void PlayMuzzleAndSfx(AudioClip sfx)
        {
            if (muzzleFx) muzzleFx.Play();
            if (audioSrc && sfx) audioSrc.PlayOneShot(sfx);
        }

        protected void PingAttention()
        {
            var pos = muzzle ? muzzle.position : (shootCam ? shootCam.transform.position : transform.position);
            ManifestAttention.Ping(pos, stats != null ? stats.noise : 1f, 6f);
        }

        protected void KickRecoil()
        {
            if (!stats || !muzzle) return;
            transform.localPosition -= transform.forward * stats.recoilKick;
        }

        protected Vector3 GetSpreadDirection(float baseSpreadDegress)
        {
            var direction = shootCam ? shootCam.transform.forward : transform.forward;
            direction = Quaternion.AngleAxis(Random.Range(-baseSpreadDegress, baseSpreadDegress), shootCam ? shootCam.transform.up : transform.up) * direction;
            direction = Quaternion.AngleAxis(Random.Range(-baseSpreadDegress, baseSpreadDegress), shootCam ? shootCam.transform.right : transform.up) * direction;
            return direction;
        }

        protected void ApplyManifestDamage(GameObject hitGO, float damageOverride = -1f)
        {
            var manifest = hitGO.GetComponentInParent<AI.ManifestController>();
            if (!manifest || stats == null) return;
            var damage = (damageOverride >= 0f) ? damageOverride : stats.damage;
            manifest.ApplyDamage(damage, stats.staggerMultiplier, stats.staggerThreshold);
        }
        
        protected void SpawnEjection(GameObject prefab, Transform port, Vector3 localVelocity, Vector3 localAngularVelocity)
        {
            if (!prefab || !port) return;
            var gameObject = Instantiate(prefab, port.position, port.rotation);
            var rigidBody = gameObject.GetComponent<Rigidbody>();
            if (!rigidBody) rigidBody = gameObject.AddComponent<Rigidbody>();

            rigidBody.linearVelocity = port.TransformDirection(localVelocity);
            rigidBody.angularVelocity = port.TransformDirection(localAngularVelocity);
        }
    }
}