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

        private Vector2 _movementRead;

        private void OnMovement(InputValue value)
        {
            _movementRead = value.Get<Vector2>();
        }

        private void FixedUpdate()
        {
            _main.MovePosition(_main.position + _movementRead * _leftRightMovementMultiply * Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            _input.actions[MOVEMENT].Enable();
        }
    }
}