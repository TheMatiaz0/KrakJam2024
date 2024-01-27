using UnityEngine;
using UnityEngine.InputSystem;
namespace KrakJam2024
{
    public class PlayerMovement : MonoBehaviour
    {
        public const string PLAYER_MOVE = "Player/Walk";

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private float _movementMultiply;
        [SerializeField] private float _movementRead;

        private PlayerView _view;

        private void Awake()
        {
            _view = GetComponentInChildren<PlayerView>();
        }

        private void OnEnable()
        {
            _input.actions[PLAYER_MOVE].Enable();
        }

        private void OnDisable()
        {
            _input.actions[PLAYER_MOVE].Disable();
        }

        private void OnWalk(InputValue value)
        {
            _movementRead = value.Get<float>();
            UpdateView(_movementRead);
        }

        private void FixedUpdate()
        {
            // if (Mathf.Abs(_movementRead) < Mathf.Epsilon)
            // {
            //     _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x * (-1f * _movementMultiply * Time.fixedDeltaTime), 0f));
            // }
            // else
            {
                _rigidbody.AddForce(new Vector2(_movementRead * _movementMultiply * Time.fixedDeltaTime, 0f));
            }
        }

        private void UpdateView(float movement)
        {
            _view.Look(movement);
        }
    }
}