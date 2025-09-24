using UnityEngine;
using MR.Systems.Input;

namespace MR.Systems.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Move")]
        public float walkSpeed = 3.2f;
        public float sprintSpeed = 6.0f;
        public float gravity = -19.6f;

        [Header("Camera")]
        public Transform cameraRoot;
        public float mouseSensitivity = 1.0f;
        public float maxPitch = 75.0f;
        public float minPitch = -75.0f;

        private CharacterController characterController;
        private float pitch;
        private Vector3 velocity;

        void Awake() => characterController = GetComponent<CharacterController>();

        void Update()
        {
            var input = InputR.Move.normalized;
            var speed = InputR.Sprint ? sprintSpeed : walkSpeed;
            var move = (transform.right * input.x + transform.forward * input.y) * speed;
            characterController.Move(move * Time.deltaTime);

            if (characterController.isGrounded && velocity.y < 0) velocity.y = -2f;
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            var look = InputR.Look * mouseSensitivity;
            pitch = Mathf.Clamp(pitch - look.y, minPitch, maxPitch);
            cameraRoot.localEulerAngles = new Vector3(pitch, 0, 0);
            transform.Rotate(Vector3.up, look.x);
        }
    }
}