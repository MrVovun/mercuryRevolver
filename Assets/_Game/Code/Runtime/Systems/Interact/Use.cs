using UnityEngine;
using MR.Core;

namespace MR.Systems.Use
{
    public class ConsumableUse : MonoBehaviour
    {
        public string itemId;
        public void UseOnce()
        {
            // centralize consume semantics here; play SFX/VFX as needed.
            if (Services.Instance.Inventory.Consume(itemId, 1))
            {
                // TODO: apply effect (heal, buff)
            }
        }
    }
}