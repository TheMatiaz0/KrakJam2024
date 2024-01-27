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

        private Vector2 _movementRead;

        private void OnMovement(InputValue value)
        {
            _movementRead = value.Get<Vector2>();
        }

        private void FixedUpdate()
        {
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
    }
}