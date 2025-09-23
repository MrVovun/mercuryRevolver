using UnityEngine;

namespace MR.Systems.Interact
{
    public abstract class Interactable : MonoBehaviour
    {
        [TextArea] public string prompt = "Interact";
        public virtual string Prompt => prompt;
        public abstract void Interact (GameObject interactor);
        }
}