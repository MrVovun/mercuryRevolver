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
    }
}