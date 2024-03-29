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
        private Material _pixaWixaBezFixa;
        [SerializeField]
        private VolumeProfile _volumeProfile;
        [SerializeField]
        private Wench _wench;
        [SerializeField]
        private GameObject _llamaEncounter;
        [SerializeField]
        private CratesController[] _cratesController;

        [SerializeField] private BiggerHead _biggerHead;

        private PhysicsMaterial2D cachedMaterial;

        private float _totalCatHappiness;

        public float StartHappiness => _startCatHappiness;
        public float GameOverHappiness => _gameOverCatHappiness;
        public float WinHappiness => _winCatHappiness;
        public CratesController[] CratesControllers => _cratesController;

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

        private void Start()
        {
            ResetVfx();
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
                    yield return ExecuteUpsideDownSpin();
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

                    flashBangMaterial.DOFloat(2.0f, "_Contrast", 0.3f).SetEase(Ease.Linear).SetLink(gameObject);
                    yield return new WaitForSeconds(0.3f);
                    flashBangMaterial.DOFloat(0.0f, "_Contrast", 4.0f).SetEase(Ease.Linear).SetLink(gameObject);

                    break;
                case ItemType.BigHead:
                    _biggerHead.Run();
                    break;
                case ItemType.Box:
                    _timePausedFor = 3f;
                    break;

                case ItemType.BlackAndWhite:
                    yield return ExecuteBlackAndWhite();
                    break;

                case ItemType.Wool:
                    _wench.Wool();
                    break;

                case ItemType.Paprika:
                    _biggerHead.Paprika();
                    break;

                case ItemType.Mirror:
                    yield return ExecuteMirrorEffects();
                    break;

                case ItemType.XMas:
                    foreach (var cc in CratesControllers)
                    {
                        foreach (var crateTransform in cc.AllCrates)
                        {
                            var crateObject = crateTransform.GetComponent<Crate>();
                            crateObject.Take();
                            crateObject.GetComponent<Rigidbody2D>().constraints = 0;
                            crateObject.Throw(Vector2.down);
                            Debug.Log(crateObject.gameObject.ToString());
                        }
                    }
                    break;

                case ItemType.PilledPillingPill:
                    yield return StartThePixaWixaWithoutFixa();
                    break;

                default:
                    Debug.Log("Not implemented : " + item.ItemType);
                    break;
            }
        }

        private IEnumerator ExecuteUpsideDownSpin()
        {
            yield return new WaitForSeconds(1.0f);

            spinFlipMaterial.DOFloat(10.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.InCubic).SetLink(gameObject);
            yield return new WaitForSeconds(0.3f);
            spinFlipMaterial.SetInt("_FlipUpsideDown", 1);
            spinFlipMaterial.SetFloat("_Spiral_Multiplier", -10.0f);
            spinFlipMaterial.DOFloat(0.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.OutCubic).SetLink(gameObject);

            yield return new WaitForSeconds(timeForCooldownedEffects);

            spinFlipMaterial.DOFloat(10.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.InCubic).SetLink(gameObject);
            yield return new WaitForSeconds(0.3f);
            spinFlipMaterial.SetInt("_FlipUpsideDown", 0);
            spinFlipMaterial.SetFloat("_Spiral_Multiplier", -10.0f);
            spinFlipMaterial.DOFloat(0.0f, "_Spiral_Multiplier", 0.3f).SetEase(Ease.OutCubic).SetLink(gameObject);
        }

        private IEnumerator ExecuteBlackAndWhite()
        {
            yield return new WaitForSeconds(0.8f);

            flashBangMaterial.DOFloat(1.0f, "_Contrast", 0.3f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(0.3f);
            flashBangMaterial.DOFloat(0.0f, "_Contrast", 0.3f).SetEase(Ease.Linear).SetLink(gameObject);
            _blackAndWhite.SetInt("_Enabled", 1);
            if (_volumeProfile.TryGet(out FilmGrain filmGrain))
            {
                filmGrain.active = true;
            }

            yield return new WaitForSeconds(10f);

            flashBangMaterial.DOFloat(1.0f, "_Contrast", 0.3f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(0.3f);
            flashBangMaterial.DOFloat(0.0f, "_Contrast", 0.3f).SetEase(Ease.Linear).SetLink(gameObject);
            filmGrain.active = false;
            _blackAndWhite.SetInt("_Enabled", 0);
        }

        private IEnumerator ExecuteMirrorEffects()
        {
            yield return new WaitForSeconds(0.8f);
            spinFlipMaterial.DOFloat(200.0f, "_Mirror_Folding_Frequency", 4f).SetEase(Ease.InOutCubic).SetLink(gameObject);
            yield return new WaitForSeconds(4.0f);
            spinFlipMaterial.DOFloat(-200.0f, "_Mirror_Folding_Frequency", 4f).SetEase(Ease.InOutCubic).SetLink(gameObject);
            yield return new WaitForSeconds(4.0f);
            spinFlipMaterial.DOFloat(0.0f, "_Mirror_Folding_Frequency", 4f).SetEase(Ease.InOutCubic).SetLink(gameObject);
        }

        private IEnumerator StartThePixaWixaWithoutFixa()
        {
            yield return new WaitForSeconds(0.8f);

            var initialIntensity = _pixaWixaBezFixa.GetFloat("_WiggleIntensity");

            _pixaWixaBezFixa.DOFloat(0.12f, "_WiggleIntensity", 4f).SetEase(Ease.InOutCubic).SetLink(gameObject);
            _pixaWixaBezFixa.DOFloat(1.0f, "_HueShiftValue", 4f).SetEase(Ease.InOutCubic).SetLink(gameObject);
            yield return new WaitForSeconds(10.0f);

            _pixaWixaBezFixa.DOFloat(initialIntensity, "_WiggleIntensity", 2f).SetEase(Ease.InOutCubic).SetLink(gameObject);
            _pixaWixaBezFixa.DOFloat(0.0f, "_HueShiftValue", 2f).SetEase(Ease.InOutCubic).SetLink(gameObject);

        }

        private void OnDestroy()
        {
            ResetVfx();
        }

        private void ResetVfx()
        {
            spinFlipMaterial.SetInt("_FlipUpsideDown", 0);
            spinFlipMaterial.SetFloat("_Spiral_Multiplier", 0.0f);

            flashBangMaterial.SetFloat("_Contrast", 0.0f);
            _blackAndWhite.SetInt("_Enabled", 0);

            spinFlipMaterial.SetFloat("_Mirror_Folding_Frequency", 0f);

            _pixaWixaBezFixa.SetFloat("_WiggleIntensity", 0.01f);
            _pixaWixaBezFixa.SetFloat("_HueShiftValue", 0f);

            Time.timeScale = 1;

            if (_volumeProfile.TryGet(out FilmGrain filmGrain))
            {
                filmGrain.active = false;
            }
        }
    }
}