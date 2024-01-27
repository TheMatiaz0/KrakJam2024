using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace KrakJam2024
{
    public class ThrowPowerRenderer : MonoBehaviour
    {
        [SerializeField] private PlayerItemHolder _itemHolder;
        [SerializeField] private LineRenderer _line;

        [SerializeField] private bool createParabola;
        [SerializeField] private LayerMask parabolaMask;

        private void Update()
        {
            var shouldWork = _itemHolder.CanThrow;
            _line.enabled = shouldWork;
            if (!shouldWork)
                return;


            List<Vector3> linePoints;
            if (createParabola)
            {
                linePoints = CreateParabolaPositions();
            }
            else
            {
                linePoints = CreatePowerIndicatorPositions();
            }

            _line.positionCount = linePoints.Count;
            _line.SetPositions(linePoints.ToArray());
        }

        private List<Vector3> CreatePowerIndicatorPositions()
        {
            var position = transform.position;

            return new(){
                position,
                Vector3.Lerp(position, _itemHolder.ThrowTarget.position, _itemHolder.CurrentPowerForIndicator)
            };
        }

        private List<Vector3> CreateParabolaPositions()
        {
            var points = new List<Vector3>();

            var position = transform.position;
            var throwVel = _itemHolder.ThrowDirection * _itemHolder.CurrentPower / _itemHolder.HeldItem.Mass;

            const int maxIterations = 60;
            const float simplifiedDeltaTime = 0.1f; // to minimize calculations
            for(int i = 0; i < maxIterations; i++)
            {
                var targetPosition = position + throwVel * simplifiedDeltaTime;
                var hit = Physics2D.Linecast(position, targetPosition, parabolaMask);
                if (hit.collider != null)
                {
                    points.Add(hit.point);
                    break;
                }
                else 
                {
                    points.Add(position);
                    position = targetPosition;
                    throwVel += (Vector3)(Physics2D.gravity * simplifiedDeltaTime);
                }
            }

            return points;
        }
    }
}