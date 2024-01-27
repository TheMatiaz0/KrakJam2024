using UnityEngine;
using UnityEngine.InputSystem;
namespace KrakJam2024
{
    public class PlayerItemHolder : MonoBehaviour
    {
        [SerializeField] private Transform _holdHere;
        [SerializeField] private Item _currentHeldItem;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private float _throwPowerMultiply = 100f;
        [SerializeField] private Animator _animator;
        private PlayerView _playerView;

        private Item _lastRegisteredItem;

        [SerializeField] private Transform _leftThrow, _rightThrow;

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
            }
            else if (_currentHeldItem != null)
            {
                _animator.SetBool("IsHoldingItem", false);
                _currentHeldItem.Throw(GetThrowVector().normalized * _throwPowerMultiply);
                _currentHeldItem = null;
            }
        }

        private void FixedUpdate()
        {
            if (_currentHeldItem != null)
            {
                _currentHeldItem.MoveTo(_holdHere);
            }
        }

        private Vector2 GetThrowVector()
        {
            var direction = _playerView.LastDirection < 0f? _leftThrow : _rightThrow;
            return direction.position - _holdHere.position;
        }
    }
}