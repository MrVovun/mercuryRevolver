using UnityEngine;
using UnityEngine.AI;
using MR.Core;

namespace MR.Systems.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ManifestController : MonoBehaviour
    {
        public Cases.ManifestArchetype data;
        public Transform target;
        public float detectionRadius = 20.0f;
        public float engageRadius = 4.0f;

        private NavMeshAgent agent;
        private float staggerTimer;
        private enum State
        {
            Prowl,
            Chase,
            Enraged,
            Stagger
        }
        private State currentState = State.Prowl;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = data.baseSpeed;
        }
        void Update()
        {
            if (Time.time < staggerTimer) return;

            if (Game.Instance.State == GameState.Countdown && currentState != State.Stagger)
            {
                currentState = State.Enraged;
                agent.speed = data.engageSpeed;
            }

            if (ManifestAttention.TryConsume(out var lure))
            {
                agent.SetDestination(lure);
                currentState = State.Chase;
                return;
            }

            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < detectionRadius)
            {
                agent.SetDestination(target.position); currentState = State.Chase;
            }
            else if (currentState != State.Prowl) currentState = State.Prowl;
        }
        public void Stagger()
        {
            staggerTimer = Time.time + data.staggerSeconds;
            currentState = State.Stagger;
            // Add stagger animation or effects here
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player") && currentState != State.Stagger)
            {
                // Handle player collision (e.g., deal damage)
            }
        }
    }
}
    