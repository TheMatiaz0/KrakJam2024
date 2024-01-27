using UnityEngine;
namespace KrakJam2024
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;

        private PlayerOwnerTransmitter _owner;

        public void Take()
        {
            _body.velocity = Vector2.zero;
            _body.angularVelocity = 0f;
            _body.isKinematic = true;
        }

        public void Throw(Vector2 throwVector)
        {
            _body.isKinematic = false;
            _body.AddForce(throwVector, ForceMode2D.Impulse);
            _body.angularVelocity = Random.Range(-360f, 360f);
        }

        // public abstract void Use();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_body.isKinematic)
                return;

            if (other.TryGetComponent<PlayerOwnerTransmitter>(out var ownerTransmitter))
            {
                _owner = ownerTransmitter;
                ownerTransmitter.Player?.RegisterItemOnGround(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerOwnerTransmitter>(out var ownerTransmitter))
            {
                _owner = ownerTransmitter;
                ownerTransmitter.Player?.UnregisterItemOnGround(this);
            }
        }
        public void MoveTo(Transform holdHere)
        {
            _body.MovePosition(holdHere.position);
        }
    }
}