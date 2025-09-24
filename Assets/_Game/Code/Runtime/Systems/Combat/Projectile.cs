using UnityEngine;
using MR.Systems.AI;

namespace MR.Systems.Combat
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 10f;
        public float lifetime = 5f;
        public float noise = 1f;

        void Start() => Destroy(gameObject, lifetime);
        void Update() => transform.position += transform.forward * speed * Time.deltaTime;
        void OnCollisionEnter(Collision collision)
        {
            ManifestAttention.Ping(transform.position, noise);
            // Logic, Impacts, Effects, etc.
            Destroy(gameObject);
        }
    }
}