using UnityEngine;
namespace KrakJam2024
{
    public class PawRotator : MonoBehaviour
    {
        [SerializeField] private Transform _transformA, _transformB;
        [SerializeField] private float _diff;
        
        private void Update()
        {
            Vector2 direction = _transformB.position - _transformA.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // For example, to apply this rotation to transformA
            _transformA.rotation = Quaternion.Euler(0, 0, angle + _diff);
        }
    }
}