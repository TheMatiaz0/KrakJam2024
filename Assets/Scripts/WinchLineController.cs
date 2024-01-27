using UnityEngine;
namespace KrakJam2024
{
    public class WinchLineController : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private Transform[] _line;

        private void Start()
        {
            _lineRenderer.positionCount = _line.Length;
        }
        
        private void Update()
        {
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, _line[i].position);
            }
        }
    }
}