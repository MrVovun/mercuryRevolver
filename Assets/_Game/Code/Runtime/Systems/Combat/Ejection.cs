using UnityEngine;
using MR.Systems.AI;

namespace MR.Systems.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class Ejection : MonoBehaviour
    {
        [Header("Noise")]
        public float baseNoise = 0.6f;          // how strong the ping is
        public float velocityScale = 0.25f;     // scales noise by impact velocity
        public float minPingVelocity = 0.7f;    // ignore tiny taps
        public float ttlSeconds = 8f;           // auto-destroy to keep scene clean

        [Header("Audio")]
        public AudioClip[] impactClips;
        public float volume = 0.8f;

        private bool pinged;
        private AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Destroy(gameObject, ttlSeconds);
        }
        void OllisionEnter(Collision collision)
        {
            if (pinged) return; // only ping once
            float velocity = collision.relativeVelocity.magnitude;
            if (velocity < minPingVelocity) return; // ignore tiny taps

            //play one-shot sound
            if (impactClips != null && impactClips.Length > 0)
            {
                AudioClip clip = impactClips[Random.Range(0, impactClips.Length)];
                if (clip) audioSource.PlayOneShot(clip, volume);
            }
            
            // Attract Manifest based on impact strength
            ManifestAttention.Ping(collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position,
            baseNoise + velocity * velocityScale, 5f);
            pinged = true;
        }
    }
}