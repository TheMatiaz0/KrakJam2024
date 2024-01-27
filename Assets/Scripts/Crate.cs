using System.Collections.Generic;
using UnityEngine;
namespace KrakJam2024
{
    [SelectionBase]
    public class Crate : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private List<Item> _itemPrefabs;
        private CratesController _controller;

        public void Take()
        {
            _body.velocity = Vector2.zero;
            _body.angularVelocity = 0f;
            _body.isKinematic = true;
            _controller.Unregister(gameObject.transform);
        }

        public void Throw(Vector2 throwVector)
        {
            _body.isKinematic = false;
            _body.AddForce(throwVector * 80f, ForceMode2D.Impulse);
            _body.angularVelocity = Random.Range(-360f, 360f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Silent"))
            {
                Destroy(gameObject);
                return;
            }
            
            var randomItem = _itemPrefabs[Random.Range(0, _itemPrefabs.Count)];
            Instantiate(randomItem, transform.position, Quaternion.identity);
            randomItem.GetComponent<Rigidbody2D>().velocity = -_body.velocity * 0.5f;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (_controller)
                _controller.Unregister(transform);
        }

        public void SetController(CratesController cratesController)
        {
            _controller = cratesController;
        }
    }
}