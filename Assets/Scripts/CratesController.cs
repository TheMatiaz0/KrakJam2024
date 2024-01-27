using System.Collections.Generic;
using UnityEngine;
namespace KrakJam2024
{
    public class CratesController : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _speedPerSecond;
        [SerializeField] private float _spawnFreqMin;
        [SerializeField] private float _spawnFreqMax;

        private float _toNextSpawn;
        private List<Transform> _registered = new();

        public void Unregister(Transform go)
        {
            _registered.Remove(go.transform);
        }

        private void Update()
        {
            foreach (var t in _registered)
            {
                t.position += new Vector3(_speedPerSecond * Time.deltaTime, Mathf.Sin(Time.time * 3f) * Time.deltaTime, 0);
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
            _registered.Add(go.transform);
        }
    }
}