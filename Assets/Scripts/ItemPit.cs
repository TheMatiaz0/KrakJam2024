using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakJam2024
{
    public class ItemPit : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Item>(out var item))
            {
                // item.Use();
            }

            // 1. Check if item was throwed by player recently
            // 2. Destroy the item
            // 3. Use its effect (abstract OnUse)
        }

        private void OnTriggerExit2D(Collider2D other)
        {

        }
    }
}
