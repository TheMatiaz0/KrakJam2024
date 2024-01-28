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

        private void Awake()
        {
            itemSystem.OnHappinessUp += ItemSystem_OnHappinessUp;
            itemSystem.OnHappinessDown += ItemSystem_OnHappinessDown;
        }

        private void ItemSystem_OnHappinessUp(float addedValue)
        {
            CookGood();
            SetStage();
        }

        private void ItemSystem_OnHappinessDown(float addedValue)
        {
            CookBad();
            SetStage();
        }

        private void OnDestroy()
        {
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
            Debug.Log(GetHighest() + 1);
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
