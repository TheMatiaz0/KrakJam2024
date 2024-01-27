using UnityEngine;
using UnityEngine.InputSystem;
namespace KrakJam2024
{
    public class WrenchItemCatcher : MonoBehaviour
    {
        private const string CATCH_ACTION = "Wench/Catch";

        [SerializeField] private Item _holdItem;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Transform _catchPoint;
        [SerializeField] private LayerMask _catchLayerMask;
        [SerializeField] private Rigidbody2D _hookBody;

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
                Debug.Log("CATCH ACTION)");
                var itemCollider = Physics2D.OverlapCircle(_catchPoint.position, .33f, _catchLayerMask);

                if (itemCollider != null && itemCollider.GetComponentInParent<Item>())
                {
                    var item = itemCollider.GetComponentInParent<Item>();
                    _holdItem = item;
                    _holdItem.transform.SetParent(_catchPoint);
                    _holdItem.Take();
                    _holdItem.LastOwner = null;
                    _anim.SetBool("Hold", true);
                }
                else
                {
                    Debug.Log("NO ITEM");
                }

            }
            if (_input.actions[CATCH_ACTION].WasReleasedThisFrame())
            {
                if (_holdItem)
                {
                    _anim.SetBool("Hold", false);
                    _holdItem.transform.SetParent(null);
                    _holdItem.Throw(_hookBody.velocity);
                    _holdItem = null;
                }
            }
        }
    }
}