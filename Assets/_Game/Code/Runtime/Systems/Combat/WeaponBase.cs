using UnityEngine;
using MR.Systems.AI;
using MR.Core;

namespace MR.Systems.Combat
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Stats")]
        public float damage = 10f;
        public float staggerMultiplier = 1f; //heavy weapons set >1
        public float staggerThreshold = 30f;    // handguns: accumulate up to this to stagger


        public float attackDuration = 0.5f;
        public float attackCooldown = 1.0f;

        protected float busyUntil = 0f; // Source of truth for ready state

        public abstract void Attack();

        protected void ApplyHit(GameObject target)
        {
            //if target tag == Manifest
            var manifest = target.GetComponentInParent<ManifestController>();
            if (manifest != null)
            {
                manifest.ApplyDamage(damage, staggerMultiplier, staggerThreshold);
            }
        }
    }
}