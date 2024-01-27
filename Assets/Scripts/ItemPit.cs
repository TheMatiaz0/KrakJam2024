using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace KrakJam2024
{
    public class ItemPit : MonoBehaviour
    {
        [SerializeField]
        private ItemSystem _itemSystem;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject);
            if (other.TryGetComponent<Item>(out var item))
            {
                if (IsItemThrownByPlayer(item) && !item.Held)
                {
                    _itemSystem.Do(item);

                    // var playerGO = item.Owner.Player.gameObject;
                    Destroy(item.gameObject);
                }
            }

            // 1. Check if item was throwed by player recently
            // 2. Destroy the item
            // 3. Use its effect (abstract OnUse)
        }

        private bool IsItemThrownByPlayer(Item item)
        {
            return item.LastOwner != null;
        }

        private void OnTriggerExit2D(Collider2D other)
        {

        }
    }
}
