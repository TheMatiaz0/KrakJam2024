using UnityEngine;
namespace KrakJam2024
{

    public enum ItemType
    {
        UpsideDown,
        IceRink,
        Llama,
        Glitter,
        BlackAndWhite,
        Flood,
        Catnip,
        HolyWater,
        BigHead,
        Box,
        Wool,

        Paprika
    }

    public class Item : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private float _catHappinessIncrease;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private AudioClip _pickUpClip;
        [SerializeField] private AudioSource _audioSource;

        // public PlayerOwnerTransmitter LastOwner { get; set; }
        public bool CanBePutInPit { get; private set; }
        public float CatHappinessIncrease => _catHappinessIncrease;
        public ItemType ItemType => _itemType;

        public float Mass => _body.mass;

        public bool Held => _body.isKinematic;

        public void Take()
        {
            _body.velocity = Vector2.zero;
            _body.angularVelocity = 0f;
            _body.isKinematic = true;
            _audioSource.PlayOneShot(_pickUpClip);
        }

        public void Throw(Vector2 throwVector)
        {
            _body.isKinematic = false;
            _body.AddForce(throwVector, ForceMode2D.Impulse);
            _body.angularVelocity = Random.Range(-360f, 360f);
            CanBePutInPit = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_body.isKinematic)
                return;

            if (other.TryGetComponent<PlayerOwnerTransmitter>(out var ownerTransmitter))
            {
                ownerTransmitter.Player?.RegisterGrabbedItem(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerOwnerTransmitter>(out var ownerTransmitter))
            {
                ownerTransmitter.Player?.UnregisterGrabbedItem(this);
            }
        }
        
        public void MoveTo(Transform holdHere)
        {
            _body.MovePosition(holdHere.position);
        }
    }
}