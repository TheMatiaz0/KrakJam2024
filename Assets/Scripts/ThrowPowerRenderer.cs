using UnityEngine;
namespace KrakJam2024
{
    public class ThrowPowerRenderer : MonoBehaviour
    {
        [SerializeField] private PlayerItemHolder _itemHolder;
        [SerializeField] private LineRenderer _line; 

        private void Update()
        {
            var shouldWork = _itemHolder.CanThrow;
            _line.enabled = shouldWork;
            if (!shouldWork)
                return;
            
            var position = transform.position;
            
            _line.SetPosition(0, position);
            _line.SetPosition(1, Vector3.Lerp(position, _itemHolder.ThrowTarget.position, _itemHolder.CurrentPower01));
        }
    }
}