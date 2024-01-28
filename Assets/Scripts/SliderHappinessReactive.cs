using UnityEngine;
using UnityEngine.UI;
namespace KrakJam2024
{
    public class SliderHappinessReactive : MonoBehaviour
    {
        [SerializeField]
        private ItemSystem _itemSystem;

        [SerializeField]
        private Slider _slider;

        private void Awake()
        {
            _slider.minValue = _itemSystem.GameOverHappiness;
            _slider.maxValue = _itemSystem.WinHappiness;

            SliderRefresh(_itemSystem.StartHappiness);

            _itemSystem.OnHappinessChanged += SliderRefresh;
        }

        private void OnDestroy()
        {
            _itemSystem.OnHappinessChanged -= SliderRefresh;
        }

        private void SliderRefresh(float value)
        {
            _slider.value = value;
        }
    }
}