using UnityEngine;
namespace KrakJam2024
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float _speed = 180f;

        void Update()
        {
            transform.Rotate(Vector3.forward, _speed);
        }
    }
}