using DG.Tweening;
using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

namespace KrakJam2024
{
    public class ItemSystem : MonoBehaviour
    {
        public event Action<float> OnHappinessUp = delegate { };
        public event Action<float> OnHappinessChanged;
        public event Action<float> OnHappinessDown = delegate { };

        [SerializeField]
        private float HappinessDecreasePerSecond = 1.2f;
        private float _timePausedFor = 0f;

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
        [SerializeField]
        private Material _blackAndWhite;
        [SerializeField]
        private VolumeProfile _volumeProfile;
        [SerializeField]
        private Wench _wench;
        [SerializeField]
        private GameObject _llamaEncounter;

        [SerializeField] private BiggerHead _biggerHead;

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
            if (_timePausedFor > 0f)
            {
                _timePausedFor -= Time.deltaTime;
                return;
            }

            // TODO: do it on crate drop
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
                    var rb2D = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
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
                    _llamaEncounter.SetActive(true);
                    yield return new WaitForSeconds(timeForCooldownedEffects);
                    _llamaEncounter.SetActive(false);
                    break;

                case ItemType.HolyWater:
                    yield return new WaitForSeconds(0.8f);

                    flashBangMaterial.DOFloat(2.0f, "_Contrast", 0.3f).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(0.3f);
                    flashBangMaterial.DOFloat(0.0f, "_Contrast", 4.0f).SetEase(Ease.Linear);

                    break;
                case ItemType.BigHead:
                    _biggerHead.Run();
                    break;
                case ItemType.Box:
                    _timePausedFor = 3f;
                    break;

                case ItemType.BlackAndWhite:
                    yield return EnableBlackAndWhite();
                    break;

                case ItemType.Wool:
                    _wench.Wool();
                    break;

                case ItemType.Paprika:
                    _biggerHead.Paprika();
                    break;

                default:
                    Debug.Log("Not implemented : " + item.ItemType);
                    break;
            }
        }

        private IEnumerator EnableBlackAndWhite()
        {
            flashBangMaterial.DOFloat(1.0f, "_Contrast", 0.3f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.3f);
            flashBangMaterial.DOFloat(0.0f, "_Contrast", 0.3f).SetEase(Ease.Linear);
            _blackAndWhite.SetInt("_Enabled", 1);
            if (_volumeProfile.TryGet(out FilmGrain filmGrain))
            {
                filmGrain.active = true;
            }

            yield return new WaitForSeconds(10f);

            flashBangMaterial.DOFloat(1.0f, "_Contrast", 0.3f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.3f);
            flashBangMaterial.DOFloat(0.0f, "_Contrast", 0.3f).SetEase(Ease.Linear);
            filmGrain.active = false;
            _blackAndWhite.SetInt("_Enabled", 0);
        }

        [Button]
        public void TestBW()
        {
            StartCoroutine(EnableBlackAndWhite());
        }


        private void OnDestroy()
        {
            spinFlipMaterial.SetInt("_FlipUpsideDown", 0);
            spinFlipMaterial.SetFloat("_Spiral_Multiplier", 0.0f);

            flashBangMaterial.SetFloat("_Contrast", 0.0f);
            _blackAndWhite.SetInt("_Enabled", 0);

            Time.timeScale = 1;

            if (_volumeProfile.TryGet(out FilmGrain filmGrain))
            {
                filmGrain.active = false;
            }
        }
    }
}