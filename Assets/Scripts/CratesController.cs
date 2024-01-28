using System.Collections.Generic;
using UnityEngine;
namespace KrakJam2024
{
    public class CratesController : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnPoint;
        [Header("Spawning properties")]
        [SerializeField] private float _speedPerSecond;
        [SerializeField] private float _spawnFreqMin;
        [SerializeField] private float _spawnFreqMax;
        [Header("Path properties")]
        [SerializeField] private float verticalWiggleSpeed;
        [SerializeField] private float verticalWiggleScale;
        [SerializeField] private float rotationSpeed;

        private float _toNextSpawn;
        private Dictionary<Transform, float> _registered = new();

        public void Unregister(Transform go)
        {
            _registered.Remove(go.transform);
        }

        private void Update()
        {
            foreach ((var t, var createTime) in _registered)
            {
                float liveTime = Time.time - createTime;
                float yOffset = Mathf.Sin(liveTime * verticalWiggleSpeed) * Time.deltaTime * verticalWiggleScale;
                t.position += new Vector3(_speedPerSecond * Time.deltaTime, yOffset, 0);

                t.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
            }

            if (_toNextSpawn <= 0)
            {
                Spawn();
                _toNextSpawn = Random.Range(_spawnFreqMin, _spawnFreqMax);
            }
            
            _toNextSpawn -= Time.deltaTime;
        }

        private void Start()
        {
            _toNextSpawn = Random.Range(_spawnFreqMin, _spawnFreqMax);
        }

        private void Spawn()
        {
            var go = Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);
            go.GetComponent<Crate>().SetController(this);
            _registered[go.transform] = Time.time;
        }
    }
}