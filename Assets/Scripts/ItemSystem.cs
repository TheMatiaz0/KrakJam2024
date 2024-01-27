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
            StartCoroutine(RunThroughTypes(item.ItemType));

            totalCatHappiness += item.CatHappinessIncrease;
        }

        private IEnumerator RunThroughTypes(ItemType type)
        {
            switch (type)
            {
                case ItemType.UpsideDown:
                    Camera.main.transform.Rotate(0, 0, 180);
                    yield return new WaitForSeconds(timeForCooldownedEffects);
                    Camera.main.transform.Rotate(0, 0, 180);
                    break;

                case ItemType.IceRink:
                    // ...
                    break;

                case ItemType.Catnip:
                    Time.timeScale = 0.5f;
                    yield return new WaitForSeconds(timeForCooldownedEffects);
                    Time.timeScale = 1f;
                    break;

                case ItemType.Llama:
                    break;

                default: break;
            }
        }
    }
}
