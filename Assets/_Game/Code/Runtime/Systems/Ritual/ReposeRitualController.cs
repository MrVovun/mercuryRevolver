using UnityEngine;
using MR.Core;

namespace MR.Systems.Ritual
{
    public class ReposeRitualController : MonoBehaviour
    {
        [Header("Setup")]
        public Transform ritualPoint;
        public float manifestRadius = 3.5f;
        public Transform manifest;
        public Transform player;

        [Header("Flow")]
        public bool active;
        public bool manifestPresent;

        void Update()
        {
            if (!active) return;

            manifestPresent = Vector3.Distance(manifest.position, ritualPoint.position) <= manifestRadius;

            if (manifestPresent)
            {
                active = false;
                Game.Instance.SetState(GameState.Repose);
                //TODO: cutscene, despawn manifest, reward player, load HQ scene
            }
        }
        public void BeginRitual() => active = true;
    }
}