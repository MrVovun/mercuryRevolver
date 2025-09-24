using UnityEngine;


namespace MR.Systems.Input
{
    public class InputR
    {
        public static Vector2 Move => new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        public static Vector2 Look => new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        public static bool Fire => UnityEngine.Input.GetMouseButtonDown(0);
        public static bool Aim => UnityEngine.Input.GetMouseButtonDown(1);
        public static bool Reload => UnityEngine.Input.GetKeyDown(KeyCode.R);
        public static bool Sprint => UnityEngine.Input.GetKey(KeyCode.LeftShift);
        public static bool Crouch => UnityEngine.Input.GetKeyDown(KeyCode.LeftControl);
        public static bool Pause => UnityEngine.Input.GetKeyDown(KeyCode.Escape);
        public static bool Interact => UnityEngine.Input.GetKeyDown(KeyCode.E);
        public static bool Inventory => UnityEngine.Input.GetKeyDown(KeyCode.Tab);
    }
}