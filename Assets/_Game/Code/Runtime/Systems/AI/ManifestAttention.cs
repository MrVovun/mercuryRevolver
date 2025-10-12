using UnityEngine;

namespace MR.Systems.AI
{
    public static class ManifestAttention
    {
        private static Vector3 lastPing;
        private static float intensity;
        private static float timeToLive;  //when ping expires
        public static float decayRatePerSec = 0.5f; //intensity units per second
        public static float consumeThreshold = 0.4f;

        public static void Ping(Vector3 position, float amount, float timeToLive = 2f)
        {
            lastPing = position;
            intensity = Mathf.Max(0f, amount);
            timeToLive = Mathf.Max(timeToLive, Time.time + timeToLive);
        }

        public static void Tick()
        {
            if (Time.time > timeToLive) { intensity = 0f; return; }
            if (intensity > 0f)
                intensity = Mathf.Max(0f, intensity - decayRatePerSec * Time.deltaTime);
        }

        public static bool TryConsume(out Vector3 position)
        {
            Tick();
            if (intensity > consumeThreshold)
            {
                position = lastPing;
                intensity = 0f; //consume
                return true;
            }
            position = default;
            return false;
        }
    }
}