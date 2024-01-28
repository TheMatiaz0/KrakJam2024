using UnityEngine;
namespace KrakJam2024
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _directionSprite;
        [SerializeField] private Transform _spriteLocators;
        public float LastDirection { get; set; }
        
        public void Look(float direction)
        {
            if (Mathf.Abs(direction) < Mathf.Epsilon)
                return;

            if (direction < 0)
            {
                _directionSprite.flipX = true;
                _spriteLocators.localScale = new(-1, 1, 1);
            }
            else
            {
                _directionSprite.flipX = false;
                _spriteLocators.localScale = new(1, 1, 1);
            }

            LastDirection = direction;
        }
    }
}