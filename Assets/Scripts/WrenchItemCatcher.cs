using UnityEngine;
using UnityEngine.InputSystem;
namespace KrakJam2024
{
    public class WrenchItemCatcher : MonoBehaviour
    {
        private const string CATCH_ACTION = "Wench/Catch";

        [SerializeField] private Crate _holdCrate;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Transform _catchPoint;
        [SerializeField] private LayerMask _catchLayerMask;
        [SerializeField] private Rigidbody2D _hookBody;
        [SerializeField] private AudioClip _catchItemSound;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private Animator _anim;

        private void OnEnable()
        {
            _input.actions[CATCH_ACTION].Enable();
        }

        private void OnDisable()
        {
            _input.actions[CATCH_ACTION].Disable();
        }
        private void FixedUpdate()
        {
            if (_input.actions[CATCH_ACTION].WasPressedThisFrame())
            {
                var itemCollider = Physics2D.OverlapCircle(_catchPoint.position, .05f, _catchLayerMask);

                if (itemCollider != null && itemCollider.GetComponentInParent<Crate>())
                {
                    Debug.Log("Fount collider");
                    
                    _holdCrate = itemCollider.GetComponentInParent<Crate>();
                    _holdCrate.transform.SetParent(_catchPoint);
                    _holdCrate.Take();
                    _holdCrate.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    _anim.SetBool("Hold", true);
                    _audioSource.PlayOneShot(_catchItemSound);
                }
                else
                {
                    Debug.Log("NO CRATE");
                }

            }
            if (_input.actions[CATCH_ACTION].WasReleasedThisFrame())
            {
                if (_holdCrate)
                {
                    _anim.SetBool("Hold", false);
                    _holdCrate.transform.SetParent(null);
                    Debug.Log("hookbody velocity" + _hookBody.velocity);
                    _holdCrate.Throw(_hookBody.velocity);
                    _holdCrate = null;
                }
            }
        }
    }
}