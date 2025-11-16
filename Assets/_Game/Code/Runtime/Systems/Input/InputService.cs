using UnityEngine;

namespace MR.Systems.Input
{
    public class InputService : MonoBehaviour
    {
        public static InputService I { get; private set; }

        [Header("General")]
        public bool enableLegacyFallback = false;
        public float mouseSensitivity = 1.0f;
        public float stickLookSensitivity = 140f;

        private Vector2 _move, _look;
        private bool _fireDown, _aimHeld, _reloadDown, _reloadHeld, _sprintHeld;
        private bool _interactDown, _crouchHeld, _useItemDown, _itemWheelHeld, _inventoryDown, _pauseDown;
        private bool _weapon1Down, _weapon2Down, _weapon3Down, _weapon4Down;

        private bool _drivenByAdapter;

        void Awake()
        {
            if (I != null) { Destroy(gameObject); return; }
            I = this;
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (_drivenByAdapter || !enableLegacyFallback) return;

            _move = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
            _look = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y")) * mouseSensitivity;

            _fireDown = UnityEngine.Input.GetMouseButtonDown(0);
            _aimHeld = UnityEngine.Input.GetMouseButton(1);
            _reloadDown = UnityEngine.Input.GetKeyDown(KeyCode.R);
            _reloadHeld = UnityEngine.Input.GetKey(KeyCode.R);
            _sprintHeld = UnityEngine.Input.GetKey(KeyCode.LeftShift);

            _interactDown = UnityEngine.Input.GetKeyDown(KeyCode.F);
            _crouchHeld = UnityEngine.Input.GetKey(KeyCode.LeftControl);
            _useItemDown = UnityEngine.Input.GetKeyDown(KeyCode.Q);
            _itemWheelHeld = UnityEngine.Input.GetKey(KeyCode.E);
            _inventoryDown = UnityEngine.Input.GetKeyDown(KeyCode.Tab);
            _pauseDown = UnityEngine.Input.GetKeyDown(KeyCode.Escape);

            _weapon1Down = UnityEngine.Input.GetKeyDown(KeyCode.Alpha1);
            _weapon2Down = UnityEngine.Input.GetKeyDown(KeyCode.Alpha2);
            _weapon3Down = UnityEngine.Input.GetKeyDown(KeyCode.Alpha3);
            _weapon4Down = UnityEngine.Input.GetKeyDown(KeyCode.Alpha4);
        }

        public Vector2 Move => _move;
        public Vector2 Look => _look;
        public bool Fire => _fireDown;
        public bool Aim => _aimHeld;
        public bool ReloadDown => _reloadDown;
        public bool ReloadHeld => _reloadHeld;
        public bool Sprint => _sprintHeld;

        public bool Interact => _interactDown;
        public bool Crouch => _crouchHeld;
        public bool UseItem => _useItemDown;
        public bool ItemWheel => _itemWheelHeld;
        public bool Inventory => _inventoryDown;
        public bool Pause => _pauseDown;

        public bool Weapon1 => _weapon1Down;
        public bool Weapon2 => _weapon2Down;
        public bool Weapon3 => _weapon3Down;
        public bool Weapon4 => _weapon4Down;

        public void SetDrivenByAdapter(bool on) => _drivenByAdapter = on;

        public void PushMove(Vector2 v) => _move = Vector2.ClampMagnitude(v, 1f);
        public void PushLook(Vector2 v) => _look = v;
        public void PushFire(bool v) => _fireDown = v;
        public void PushAim(bool v) => _aimHeld = v;
        public void PushReload(bool down, bool held) { _reloadDown = down; _reloadHeld = held; }
        public void PushSprint(bool v) => _sprintHeld = v;

        public void PushInteract(bool v) => _interactDown = v;
        public void PushCrouch(bool v) => _crouchHeld = v;
        public void PushUseItem(bool v) => _useItemDown = v;
        public void PushItemWheel(bool v) => _itemWheelHeld = v;
        public void PushInventory(bool v) => _inventoryDown = v;
        public void PushPause(bool v) => _pauseDown = v;

        public void PushWeaponSlot(bool w1, bool w2, bool w3, bool w4)
        { _weapon1Down = w1; _weapon2Down = w2; _weapon3Down = w3; _weapon4Down = w4; }
    }

    public static class InputR
    {
        private static InputService S => InputService.I;
        public static Vector2 Move => S?.Move ?? Vector2.zero;
        public static Vector2 Look => S?.Look ?? Vector2.zero;
        public static bool Fire => S != null && S.Fire;
        public static bool Aim => S != null && S.Aim;
        public static bool ReloadDown => S != null && S.ReloadDown;
        public static bool ReloadHeld => S != null && S.ReloadHeld;
        public static bool Sprint => S != null && S.Sprint;

        public static bool Interact => S != null && S.Interact;
        public static bool Crouch => S != null && S.Crouch;
        public static bool UseItem => S != null && S.UseItem;
        public static bool ItemWheel => S != null && S.ItemWheel;
        public static bool Inventory => S != null && S.Inventory;
        public static bool Pause => S != null && S.Pause;

        public static bool Weapon1 => S != null && S.Weapon1;
        public static bool Weapon2 => S != null && S.Weapon2;
        public static bool Weapon3 => S != null && S.Weapon3;
        public static bool Weapon4 => S != null && S.Weapon4;
    }
}