using System.Collections.Generic;
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
        [SerializeField] private Transform _visuallyHoldThere;
        [SerializeField] private Item _currentHeldItem;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private float _throwPowerMultiply = 100f;
        [SerializeField] private Animator _animator;
        [Header("Sounds")]
        [SerializeField] private AudioClip _throwSound;
        [SerializeField] private AudioClip _buildupSound;
        [SerializeField] private AudioSource _audioSource;

        private PlayerView _playerView;
        private bool _takenThisFrame;
        private bool _canThrow;


        private List<Item> _registeredItemsStack = new(); 

        private void Awake()
        {
            _playerView = GetComponentInChildren<PlayerView>();
        }

        public void RegisterGrabbedItem(Item item)
        {
            _registeredItemsStack.Add(item);
        }

        public void UnregisterGrabbedItem(Item item)
        {
            _registeredItemsStack.Remove(item);
        }

        private void OnTake()
        {
            if (_registeredItemsStack.Count == 0)
                return;
            
            if (_registeredItemsStack.Count > 0 && _currentHeldItem == null)
            {
                _animator.SetBool("IsHoldingItem", true);
                var item = _registeredItemsStack[^1];
                _registeredItemsStack.Remove(item);
                item.Take();
                _currentHeldItem = item;
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

            _currentHeldItem.MoveTo(_holdHere);
            _currentHeldItem.Throw(GetThrowVector() * _currentPower);
            UnregisterGrabbedItem(_currentHeldItem);
            _currentHeldItem = null;
            _canThrow = false;
            _animator.SetBool("IsHoldingItem", false);
        }

        private void FixedUpdate()
        {
            if (_currentHeldItem != null)
            {
                _currentHeldItem.MoveTo(_visuallyHoldThere);
                if (_takenThisFrame)
                {
                    _takenThisFrame = false;
                    return;
                }

                if (_canThrow)
                {
                    if (_input.actions[TAKE_ACTION].WasReleasedThisFrame())
                    {
                        _audioSource.Stop();
                        ThrowHeldItem();
                        _audioSource.PlayOneShot(_throwSound);
                    }
                    else
                    {
                        _audioSource.clip = _buildupSound;
                        _audioSource.loop = true;
                        _audioSource.Play();
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