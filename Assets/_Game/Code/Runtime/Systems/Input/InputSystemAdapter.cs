#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace MR.Systems.Input
{
    public class InputSystemAdapter : MonoBehaviour
    {
        [Header("Actions")]
        public InputActionAsset actionsAsset;
        public string actionMapName = "Gameplay";

        [Header("Look Scaling")]
        public float mouseSensitivity = 1.0f;
        public float stickLookSensitivity = 140f;

        private InputAction aMove, aLook, aFire, aAim, aReload, aSprint, aInteract, aCrouch, aUseItem, aItemWheel, aInventory, aPause;
        private InputAction aWeapon1, aWeapon2, aWeapon3, aWeapon4;

        void OnEnable()
        {
            if (!actionsAsset) { Debug.LogError("InputSystemAdapter: actionsAsset not set"); enabled = false; return; }
            var map = actionsAsset.FindActionMap(actionMapName, true);

            aMove = map.FindAction("Move", true);
            aLook = map.FindAction("Look", true);
            aFire = map.FindAction("Fire", true);
            aAim = map.FindAction("Aim", true);
            aReload = map.FindAction("Reload", true);
            aSprint = map.FindAction("Sprint", true);
            aInteract = map.FindAction("Interact", true);
            aCrouch = map.FindAction("Crouch", true);
            aUseItem = map.FindAction("UseItem", true);
            aItemWheel = map.FindAction("ItemWheel", true);
            aInventory = map.FindAction("Inventory", true);
            aPause = map.FindAction("Pause", true);

            aWeapon1 = map.FindAction("Weapon1", true);
            aWeapon2 = map.FindAction("Weapon2", true);
            aWeapon3 = map.FindAction("Weapon3", true);
            aWeapon4 = map.FindAction("Weapon4", true);

            map.Enable();
            InputService.I.SetDrivenByAdapter(true);
        }

        void OnDisable()
        {
            actionsAsset?.FindActionMap(actionMapName, false)?.Disable();
            if (InputService.I) InputService.I.SetDrivenByAdapter(false);
        }

        void Update()
        {
            var S = InputService.I; if (S == null) return;

            S.PushMove(aMove.ReadValue<Vector2>());

            var look = aLook.ReadValue<Vector2>();
            if (Mouse.current != null && Mouse.current.delta != null && Mouse.current.delta.ReadValue() != Vector2.zero)
                look *= mouseSensitivity;
            else
                look *= stickLookSensitivity * Time.deltaTime;
            S.PushLook(look);

            S.PushFire(aFire.WasPressedThisFrame());
            S.PushAim(aAim.IsPressed());
            S.PushReload(aReload.WasPressedThisFrame(), aReload.IsPressed());
            S.PushSprint(aSprint.IsPressed());

            S.PushInteract(aInteract.WasPressedThisFrame());
            S.PushCrouch(aCrouch.IsPressed());
            S.PushUseItem(aUseItem.WasPressedThisFrame());
            S.PushItemWheel(aItemWheel.IsPressed());
            S.PushInventory(aInventory.WasPressedThisFrame());
            S.PushPause(aPause.WasPressedThisFrame());

            S.PushWeaponSlot(aWeapon1.WasPressedThisFrame(), aWeapon2.WasPressedThisFrame(),
                             aWeapon3.WasPressedThisFrame(), aWeapon4.WasPressedThisFrame());
        }
    }
}
#endif
