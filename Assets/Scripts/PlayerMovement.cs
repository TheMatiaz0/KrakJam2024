using DG.Tweening;
using NaughtyAttributes;
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
        [SerializeField] private Animator _animator;

        private PlayerView _view;
        private bool _snapped;

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
            if (_snapped) _movementRead *= .1f;
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

        [Button]
        public void Snap()
        {
            if (_snapped)
                return;

            _view.transform.DOLocalRotate(new Vector3(0f, 0f, -90f), .12f);
            _snapped = true;
            Invoke(nameof(UnSnap), 3f);
        }

        public void UnSnap()
        {
            _snapped = false;
            _rigidbody.velocity = Vector2.zero;
            _view.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), .12f);
            // _view.transform.localRotation = Quaternion.Euler(0f, 0f, 0);
        }

        private void UpdateView(float movement)
        {
            _animator.SetBool("IsWalking", movement != 0);
            _view.Look(movement);
        }
    }
}