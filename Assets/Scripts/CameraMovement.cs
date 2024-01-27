using UnityEngine;

namespace KrakJam2024
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Camera controlledCamera;

        [SerializeField] private Transform[] followedObjects;
        [SerializeField] private float followSpeed;
        [SerializeField] private float followForce;
        [SerializeField] private Vector2 followOffset;


        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, GetFollowingPosition(), followSpeed * Time.deltaTime);
        }


        private Vector3 GetFollowingPosition()
        {
            var followingPosition = GetAverageOfFollowedObjectPositions() * followForce;
            followingPosition.z = transform.position.z;
            return followingPosition + (Vector3) followOffset;
        }


        private Vector3 GetAverageOfFollowedObjectPositions()
        {
            var totalPosition = Vector3.zero;
            foreach(var followed in followedObjects) totalPosition += followed.position;
            return totalPosition / followedObjects.Length;
        }

    }
}
