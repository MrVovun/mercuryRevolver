using UnityEngine;
using UnityEngine.AI;
using MR.Core;
using MR.Systems.Cases;
using Unity.Mathematics;

namespace MR.Systems.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ManifestController : MonoBehaviour
    {
        public ManifestArchetype data;
        public Transform target;
        public float detectionRadius = 20.0f;
        public float engageRadius = 4.0f;

        private NavMeshAgent agent;
        private float staggerTimer;
        private float health;
        private float accumulatedDamage;

        private enum State
        {
            Prowl,
            Chase,
            Enraged,
            Staggered
        }
        private State currentState = State.Prowl;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = data.baseSpeed;
            health = data.virtualMaxHP;
        }
        void Update()
        {
            // Attention decay always ticking
            ManifestAttention.Tick();

            if (currentState == State.Staggered)
            {
                if (Time.time >= staggerTimer)
                {
                    currentState = (Game.Instance.State == GameState.Countdown) ? State.Enraged : State.Prowl;
                    agent.isStopped = false;
                    agent.speed = (currentState == State.Enraged) ? data.engageSpeed : data.baseSpeed;
                }
                return;
            }

            // Countdown change state to Enraged
            if (Game.Instance.State == GameState.Countdown && currentState != State.Staggered)
            {
                currentState = State.Enraged;
                agent.speed = data.engageSpeed;
            }

            //Attention lure
            if (ManifestAttention.TryConsume(out var lure))
            {
                agent.SetDestination(lure);
                currentState = (currentState == State.Enraged) ? State.Enraged : State.Chase;
                return;
            }

            //Regular chase/prowl
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < detectionRadius)
            {
                agent.SetDestination(target.position); currentState = State.Chase;
            }
            else if (currentState != State.Prowl && currentState != State.Enraged)
            {
                // Lost target, return to Prowl
                currentState = State.Prowl;
            }
        }

        public void ApplyDamage(float damage, float staggerMultiplier, float staggerThreshold)
        {
            // Soft HP is for audio/FX pacing; the Manifest doesn't "die."
            health = Mathf.Max(0f, health - damage);

            // Accumulate for handguns/low-caliber; heavy weapons stagger by multiplier.
            accumulatedDamage += damage;

            bool shouldStagger = false;

            if (staggerMultiplier > 1f)
            {
                // Heavy weapon hit
                shouldStagger = true;
            }
            else if (accumulatedDamage >= Mathf.Max(1f, staggerThreshold))
            {
                // Handgun hit, but enough accumulated damage
                shouldStagger = true;
                accumulatedDamage = 0f; //reset accumulation
            }
            if (shouldStagger) Stagger(data.staggerSeconds * Mathf.Max(0.25f, staggerMultiplier));
        }

        public void Stagger(float seconds)
        {
            staggerTimer = Time.time + data.staggerSeconds;
            currentState = State.Staggered;
            agent.isStopped = true;
            // TODO: Add stagger animation or effects here
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player") && currentState != State.Staggered)
            {
                //TODO: Handle player collision (deal damage)
            }
        }
    }
}
