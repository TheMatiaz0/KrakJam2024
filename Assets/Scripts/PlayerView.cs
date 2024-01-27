using UnityEngine;
namespace KrakJam2024
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _directionSprite;
        public float LastDirection { get; set; }
        
        public void Look(float direction)
        {
            if (Mathf.Abs(direction) < Mathf.Epsilon)
                return;

            if (direction < 0)
            {
                _directionSprite.flipX = true;
            }
            else
            {
                _directionSprite.flipX = false;
            }
            
            LastDirection = direction;
        }
    }
}