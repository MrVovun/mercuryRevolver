using UnityEngine;

namespace MR.Systems.Tools
{
    public abstract class ToolBase : MonoBehaviour
    {
        public string toolID;
        public string displayName;

        [Header("Timings")]
        public float rechargeTime = 1.0f;
        public float useDuration = 1.0f;

        protected float busyUntil = 0f; // Source of truth for ready state
        public bool active;

        public virtual bool IsReady => Time.time >= busyUntil && !active;

        public void TryUse()
        {
            if (!IsReady) return;
            active = true;
            busyUntil = Time.time + useDuration;
            BeginUse();
        }

        public void Tick()
        {
            if (!active) return;
            CanUse();
            if (Time.time >= busyUntil)
            {
                active = false;
                EndUse();
                busyUntil = Time.time + rechargeTime;
            }
        }

        public abstract bool CanUse();
        public abstract void BeginUse();
        public abstract void EndUse();
    }
}