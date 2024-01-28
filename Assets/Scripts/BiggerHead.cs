using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
namespace KrakJam2024
{
    public class BiggerHead : MonoBehaviour
    {
        private float _timeToDisable = 0f;
        private bool _isLarge = false;
        [SerializeField] private Vector3 _standardSize;
        [SerializeField] private Vector3 _largeSize;
        [SerializeField] private Vector3 _changedPos;
        [SerializeField] private Vector3 _originalPos;

        [Button]
        public void Run() => Run(5f);

        private void Start()
        {
            _originalPos = transform.localPosition;
        }

        private void Run(float addTime)
        {
            _timeToDisable += addTime;
            _isLarge = true;
            DOTween.Sequence()
                .Join(transform.DOScale(_largeSize, .5f))
                .Join(transform.DOLocalMove(_changedPos, .5f))
                .SetEase(Ease.OutBack)
                .Play();
        }

        private void Update()
        {
            if (!_isLarge)
                return;

            if (_timeToDisable < 0f)
            {
                _isLarge = false;
                DOTween.Sequence()
                    .Join(transform.DOScale(_standardSize, .5f))
                    .Join(transform.DOLocalMove(_originalPos, .5f))
                    .SetEase(Ease.OutBack)
                    .Play();
                _timeToDisable = 0f;
            }

            _timeToDisable -= Time.deltaTime;
        }
    }
}