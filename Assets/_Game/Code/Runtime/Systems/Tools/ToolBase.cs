using UnityEngine;

namespace MR.Systems.Tools
{
    public abstract class ToolBase : MonoBehaviour
    {
        public string toolID;
        public string displayName;
        public float rechargeTime;
        public float useDuration;

        public abstract bool CanUse();
        public abstract void BeginUse();
        public abstract void EndUse();
    }
}