using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KrakJam2024
{
    public class CatHeadAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] stagesSprites;
        [SerializeField] private ItemSystem itemSystem;

        [SerializeField] private ParticleSystem badCookParticlePrefab;
        [SerializeField] private ParticleSystem gudCookParticlePrefab;

        [SerializeField] private List<int> maxValues = new();
        [SerializeField] private AudioSource audioSource;
        // [SerializeField] private AudioClip goodSound;
        // [SerializeField] private AudioClip badSound;
        [SerializeField] private AudioClip buildupSound;

        private float cachedValue;

        private void Awake()
        {
            itemSystem.OnHappinessChanged += ItemSystem_OnHappinessChanged;
            itemSystem.OnHappinessUp += ItemSystem_OnHappinessUp;
            itemSystem.OnHappinessDown += ItemSystem_OnHappinessDown;
        }

        private void ItemSystem_OnHappinessChanged(float addedValue)
        {
            SetStage();
        }

        private void ItemSystem_OnHappinessUp(float addedValue)
        {
            CookGood();
        }

        private void ItemSystem_OnHappinessDown(float addedValue)
        {
            CookBad();
        }
        
        private void OnDestroy()
        {
            itemSystem.OnHappinessChanged -= ItemSystem_OnHappinessChanged;
            itemSystem.OnHappinessUp -= ItemSystem_OnHappinessUp;
            itemSystem.OnHappinessDown -= ItemSystem_OnHappinessDown;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                CookGood();
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                CookBad();
            }
        }

        public void SetStage()
        {
            animator.SetInteger("Stage", GetHighest() + 1);
        }

        private int GetHighest()
        {
            for (int i = 0; i < maxValues.Count; i++)
            {
                var maxValue = maxValues[i];
                if (maxValue > itemSystem.TotalCatHappiness)
                {
                    return i;
                }
            }
            return maxValues.Count;
        }

        private void Cook(bool good)
        {
            audioSource.PlayOneShot(buildupSound);
            // audioSource.PlayOneShot(good ? goodSound : badSound);

            animator.SetTrigger("LetHimCook");
            var particle = Instantiate(good ? gudCookParticlePrefab : badCookParticlePrefab, transform);
            Destroy(particle, 2.0f);
        }

        public void CookGood() {
            Cook(good: true);
        }

        public void CookBad()
        {
            Cook(good: false);
        }
    }
}
