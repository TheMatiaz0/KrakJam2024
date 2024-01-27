using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakJam2024
{
    public class ItemSystem : MonoBehaviour
    {
        [SerializeField]
        private float timeForCooldownedEffects = 4;

        private float totalCatHappiness;

        public void Do(Item item)
        {
            switch (item.ItemType)
            {
                case ItemType.UpsideDown:
                    Camera.main.transform.Rotate(0, 0, 180);
                    break;
                case ItemType.IceRink:
                    // ...
                    break;

                case ItemType.Catnip:
                    Time.timeScale = 0.5f;
                    break;

                default: break;
            }

            StartCoroutine(RunCooldownedEffect(item));
            totalCatHappiness += item.CatHappinessIncrease;
        }

        private IEnumerator RunCooldownedEffect(Item item)
        {
            if (item.IsCooldownedEffect)
            {
                yield return new WaitForSeconds(timeForCooldownedEffects);
            }
            else
            {
                // .. wait for animation to finish
            }
        }
    }
}
