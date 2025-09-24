using UnityEngine;
using UnityEngine.UI;
using MR.Systems.Interact;

namespace MR.UI
{
    public class HUDPrompts : MonoBehaviour
    {
        public Camera cam;
        public float range = 3f;
        public LayerMask layerMask;
        public Text promptText;

        void Update()
        {
            promptText.text = "";
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out var hit, range, layerMask))
            {
                var interactable = hit.collider.GetComponentInParent<Interactable>();
                if (interactable != null)
                {
                    promptText.text = interactable.prompt + " [E]";
                }
            }
        }
    }
}