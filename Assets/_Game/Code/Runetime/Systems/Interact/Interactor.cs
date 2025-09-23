using UnityEngine;
using MR.Systems.Input;

namespace MR.Systems.Interact
{

    public class Interactor : MonoBehaviour
    {
        public Camera cam;
        public float range = 3.0f;
        public LayerMask mask;

        private Interactable hovered;

        void Update()
        {
            hovered = null;
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out var hit, range, mask))
            {
                hovered = hit.collider.GetComponent<Interactable>();
            }
            if (hovered != null && InputR.Interact)
            {
                hovered.Interact(gameObject);
            }
        }
    }
}