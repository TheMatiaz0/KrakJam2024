using UnityEngine;
using UnityEngine.InputSystem;
namespace KrakJam2024
{
    public class PlayerItemHolder : MonoBehaviour
    {
        private const string TAKE_ACTION = "Player/Take";
        [Header("Throwing config")]
        [SerializeField] private float _throwPowerMin;
        [SerializeField] private float _throwPowerMax;
        [SerializeField] private float _powerPlusPerSecond;
        [SerializeField] private Transform _leftThrow, _rightThrow;

        private float _currentPower;
        public float CurrentPowerForIndicator => Mathf.InverseLerp(_throwPowerMin, _throwPowerMax, _currentPower);
        public float CurrentPower => _currentPower;
        public bool CanThrow => _canThrow;
        public Transform ThrowTarget => _playerView.LastDirection < 0f ? _leftThrow : _rightThrow;
        public Vector3 ThrowDirection => GetThrowVector();
        public Item HeldItem => _currentHeldItem;

        [Space]
        [SerializeField] private Transform _holdHere;
        [SerializeField] private Item _currentHeldItem;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private float _throwPowerMultiply = 100f;
        [SerializeField] private Animator _animator;
        private PlayerView _playerView;
        private bool _takenThisFrame;
        private bool _canThrow;

        private Item _lastRegisteredItem;

        private void Awake()
        {
            _playerView = GetComponentInChildren<PlayerView>();
        }

        public void RegisterItemOnGround(Item item)
        {
            _lastRegisteredItem = item;
        }

        public void UnregisterItemOnGround(Item item)
        {
            _lastRegisteredItem = null;
        }

        private void OnTake()
        {
            if (_lastRegisteredItem != null && _currentHeldItem == null)
            {
                _animator.SetBool("IsHoldingItem", true);
                _lastRegisteredItem.Take();
                _currentHeldItem = _lastRegisteredItem;
                _lastRegisteredItem = null;
                _takenThisFrame = true;
            }
            // else if (_currentHeldItem != null)
            // {
            //     _animator.SetBool("IsHoldingItem", false);
            //     _currentHeldItem.Throw(GetThrowVector().normalized * _throwPowerMultiply);
            //     _currentHeldItem = null;
            // }
        }

        private void ThrowHeldItem()
        {
            if(_currentHeldItem == null)
            {
                return;
            }

            _currentHeldItem.Throw(GetThrowVector() * _currentPower);
            UnregisterItemOnGround(_currentHeldItem);
            _currentHeldItem = null;
            _canThrow = false;
            _animator.SetBool("IsHoldingItem", false);
        }

        private void FixedUpdate()
        {
            if (_currentHeldItem != null)
            {
                _currentHeldItem.MoveTo(_holdHere);
                if (_takenThisFrame)
                {
                    _takenThisFrame = false;
                    return;
                }

                if (_canThrow)
                {
                    if (_input.actions[TAKE_ACTION].WasReleasedThisFrame())
                    {
                        ThrowHeldItem();
                    }
                    else
                    {
                        _currentPower += _powerPlusPerSecond * Time.fixedDeltaTime;
                        _currentPower = Mathf.Min(_currentPower, _throwPowerMax);
                    }
                }

                if (_input.actions[TAKE_ACTION].WasPressedThisFrame())
                {
                    _canThrow = true;
                    _currentPower = _throwPowerMin;
                }
            }
        }

        private Vector2 GetThrowVector()
        {
            return (ThrowTarget.position - _holdHere.position).normalized;
        }
    }
}