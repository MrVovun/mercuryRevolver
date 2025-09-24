using UnityEngine;

namespace MR.Systems.AI
{
    public static class ManifestAttention
    {
        private static Vector3 lastPing;
        private static float intensity;

        public static void Ping(Vector3 position, float amount)
        {
            lastPing = position;
            intensity = amount;
        }

        public static bool TryConsume(out Vector3 position)
        {
            if (intensity > 0)
            {
                position = lastPing;
                return true;
            }
            position = default;
            return false;
        }
    }
}