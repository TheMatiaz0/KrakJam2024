using UnityEngine;
using UnityEngine.InputSystem;
namespace KrakJam2024
{
    public class PlayerItemHolder : MonoBehaviour
    {
        private const string TAKE_ACTION = "Player/Take";

        [SerializeField] private Transform _holdHere;
        [SerializeField] private Item _currentHeldItem;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private float _throwPowerMultiply = 100f;
        private PlayerView _playerView;

        private Item _lastRegisteredItem;

        [SerializeField] private Transform _leftThrow, _rightThrow;

        private void Awake()
        {
            _playerView = GetComponentInChildren<PlayerView>();
        }

        public void RegisterItemOnGround(Item item)
        {
            Debug.Log("Registerd");
            _lastRegisteredItem = item;
        }

        public void UnregisterItemOnGround(Item item)
        {
            Debug.Log("UnRegisterd");
            _lastRegisteredItem = null;
        }

        private void FixedUpdate()
        {
            if (_input.actions[TAKE_ACTION].WasPressedThisFrame() && _lastRegisteredItem != null && _currentHeldItem == null)
            {
                _lastRegisteredItem.Take();
                _currentHeldItem = _lastRegisteredItem;
                _lastRegisteredItem = null;
            }
            else if (_input.actions[TAKE_ACTION].WasPressedThisFrame() && _currentHeldItem != null)
            {
                _currentHeldItem.Throw(GetThrowVector().normalized * _throwPowerMultiply);
                _currentHeldItem = null;
            }

            if (_currentHeldItem)
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