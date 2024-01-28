using DG.Tweening;
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
        [SerializeField]
        private Material spinFlipMaterial;

        private PhysicsMaterial2D cachedMaterial;

        private float _totalCatHappiness;

        public float StartHappiness => _startCatHappiness;
        public float GameOverHappiness => _gameOverCatHappiness;
        public float WinHappiness => _winCatHappiness;

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
                    yield return new WaitForSeconds(1.0f);

                    spinFlipMaterial.DOFloat(10.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.InCubic);
                    yield return new WaitForSeconds(0.3f);
                    spinFlipMaterial.SetInt("_FlipUpsideDown", 1);
                    spinFlipMaterial.SetFloat("_Spiral_Multiplier", -10.0f);
                    spinFlipMaterial.DOFloat(0.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.OutCubic);

                    yield return new WaitForSeconds(timeForCooldownedEffects);

                    spinFlipMaterial.DOFloat(10.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.InCubic);
                    yield return new WaitForSeconds(0.3f);
                    spinFlipMaterial.SetInt("_FlipUpsideDown", 0);
                    spinFlipMaterial.SetFloat("_Spiral_Multiplier", -10.0f);
                    spinFlipMaterial.DOFloat(0.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.OutCubic);

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
