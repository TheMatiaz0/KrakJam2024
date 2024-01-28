using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace KrakJam2024
{
    public class ItemSystem : MonoBehaviour
    {
        public event Action<float> OnHappinessUp = delegate { };
        public event Action<float> OnHappinessChanged;
        public event Action<float> OnHappinessDown = delegate { };

        [SerializeField]
        private float HappinessDecreasePerSecond = 1.2f;

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
        [SerializeField]
        private Material flashBangMaterial;

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
                if (Math.Abs(value - _totalCatHappiness) > Mathf.Epsilon)
                {
                    _totalCatHappiness = value;
                    OnHappinessChanged?.Invoke(_totalCatHappiness);

                    if (value <= 0)
                    {
                        _gameManager.GameOver();
                        ResetHappiness();
                        return;
                    }

                    if (value >= 100)
                    {
                        _gameManager.Win();
                        ResetHappiness();
                    }
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

        private void Update()
        {
            TotalCatHappiness -= HappinessDecreasePerSecond * Time.deltaTime;
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
                    yield return new WaitForSecondsRealtime(timeForCooldownedEffects);
                    Time.timeScale = 1f;
                    break;

                case ItemType.Llama:
                    break;

                case ItemType.HolyWater:
                    yield return new WaitForSeconds(0.8f);

                    flashBangMaterial.DOFloat(2.0f, "_Contrast", 0.3f).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(0.3f);
                    flashBangMaterial.DOFloat(0.0f, "_Contrast", 2.0f).SetEase(Ease.Linear);

                    break;

                default: break;
            }
        }


        private void OnDestroy()
        {
            spinFlipMaterial.SetInt("_FlipUpsideDown", 0);
            spinFlipMaterial.SetFloat("_Spiral_Multiplier", 0.0f);
            Time.timeScale = 1;
        }
    }
}