using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakJam2024
{
    public class ItemSystem : MonoBehaviour
    {
        public event Action<float> OnHappinessUp = delegate { };
        public event Action<float> OnHappinessDown = delegate { };

        [SerializeField]
        private float timeForCooldownedEffects = 4;
        [SerializeField]
        private PhysicsMaterial2D slipperyMaterial;
        [SerializeField]
        private float _startCatHappiness = 25;
        [SerializeField]
        private float _gameOverCatHappiness = 0;
        [SerializeField]
        private float _winCatHappiness = 100;
        [SerializeField]
        private GameManager _gameManager;

        private PhysicsMaterial2D cachedMaterial;

        private float _totalCatHappiness;
        public float TotalCatHappiness 
        {
            get => _totalCatHappiness;
            private set
            {
                if (value != _totalCatHappiness)
                {
                    if (value <= 0)
                    {
                        _gameManager.GameOver();
                        ResetHappiness();
                        return;
                    }
                    else if (value >= 100)
                    {
                        _gameManager.Win();
                        ResetHappiness();
                        return;
                    }
                    _totalCatHappiness = value;
                }
            }
        }

        private void ResetHappiness()
        {
            _totalCatHappiness = _startCatHappiness;
        }

        private void Awake()
        {
            ResetHappiness();
        }

        public void Do(Item item)
        {
            StartCoroutine(RunThroughTypes(item));

            if (item.CatHappinessIncrease > 0)
            {
                OnHappinessUp?.Invoke(item.CatHappinessIncrease);
            }
            else if (item.CatHappinessIncrease < 0)
            {
                OnHappinessDown?.Invoke(item.CatHappinessIncrease);
            }

            TotalCatHappiness += item.CatHappinessIncrease;
        }

        private IEnumerator RunThroughTypes(Item item)
        {
            // God forbid me for this crime 
            switch (item.ItemType)
            {
                case ItemType.UpsideDown:
                    Camera.main.transform.Rotate(0, 0, 180);
                    yield return new WaitForSeconds(timeForCooldownedEffects);
                    Camera.main.transform.Rotate(0, 0, 180);
                    break;

                case ItemType.IceRink:
                    var rb2D = item.LastOwner.Player.GetComponent<Rigidbody2D>();
                    cachedMaterial = rb2D.sharedMaterial;
                    rb2D.sharedMaterial = slipperyMaterial;
                    yield return new WaitForSeconds(timeForCooldownedEffects);
                    rb2D.sharedMaterial = cachedMaterial;
                    break;

                case ItemType.Catnip:
                    Time.timeScale = 1.75f;
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
