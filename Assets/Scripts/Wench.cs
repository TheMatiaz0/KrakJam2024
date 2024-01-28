using DG.Tweening;
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
        [SerializeField] private DistanceJoint2D[] _joints;
        [SerializeField] private float _minDistance, _maxDistance;

        [SerializeField] private GameObject _wool;

        [Header("Wench")]
        private float _timeToEndWool = 0f;

        private Vector2 _movementRead;

        public void Wool()
        {
            ShowWool();
            _timeToEndWool = 5f;
        }

        private void OnMovement(InputValue value)
        {
            _movementRead = value.Get<Vector2>();
        }

        private void Update()
        {
            if (_timeToEndWool > 0f)
            {
                _timeToEndWool -= Time.deltaTime;
            }
            else
            {
                HideWool();
            }
        }

        private void FixedUpdate()
        {
            if (_timeToEndWool > 0f)
                return;
            _main.MovePosition(_main.position + new Vector2(_movementRead.x * (_leftRightMovementMultiply * Time.fixedDeltaTime), 0f));
            foreach (var joint in _joints)
            {
                joint.distance += -_movementRead.y * Time.fixedDeltaTime;
                joint.distance = Mathf.Clamp(joint.distance, _minDistance, _maxDistance);
            }
        }

        private void OnEnable()
        {
            _input.actions[MOVEMENT].Enable();
        }

        private void ShowWool()
        {
            _wool.transform.localScale = Vector3.zero;
            _wool.transform.DOScale(Vector3.one * 2.5f, .12f);
        }

        private void HideWool() { _wool.transform.DOScale(Vector3.one * 2.5f, .12f); }
    }
}