using UnityEngine;

namespace KrakJam2024
{
    public class CatHeadAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] stagesSprites;

        [SerializeField] private ParticleSystem badCookParticlePrefab;
        [SerializeField] private ParticleSystem gudCookParticlePrefab;

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

        public void SetStage(int stage)
        {
            animator.SetInteger("Stage", stage);

            // stage starts at 1. don't ask why
            var stageIndex = stage - 1;

            if (stageIndex >= 0 && stageIndex < stagesSprites.Length)
            {
                spriteRenderer.sprite = stagesSprites[stageIndex];
            }
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
