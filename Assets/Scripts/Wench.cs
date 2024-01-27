using UnityEngine;
using UnityEngine.InputSystem;
namespace KrakJam2024
{
    public class Wench : MonoBehaviour
    {
        private const string MOVEMENT = "Wench/Movement";
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Rigidbody2D _main;
        [SerializeField] private float _leftRightMovementMultiply;

        private void FixedUpdate()
        {
            var read = _input.actions[MOVEMENT].ReadValue<Vector2>();
            _main.MovePosition(_main.position + read * _leftRightMovementMultiply * Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            _input.actions[MOVEMENT].Enable();
        }
    }
}