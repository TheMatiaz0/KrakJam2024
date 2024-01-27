using UnityEngine;
namespace KrakJam2024
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;

        public void Take()
        {
            _body.isKinematic = true;
        }

        public void Throw(Vector2 throwVector)
        {
            _body.isKinematic = false;
            _body.AddForce(throwVector, ForceMode2D.Impulse);
        }
    }
}