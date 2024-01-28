using System.Collections;
using System.Collections.Generic;
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

            _itemSystem.OnHappinessUp += OnHappinessUp;
            _itemSystem.OnHappinessDown += OnHappinessDown;
        }

        private void OnDestroy()
        {
            _itemSystem.OnHappinessUp -= OnHappinessUp;  
            _itemSystem.OnHappinessDown -= OnHappinessDown;
        }

        private void SliderRefresh(float value)
        {
            _slider.value = value;
        }

        private void OnHappinessUp(float value)
        {
            SliderRefresh(_itemSystem.TotalCatHappiness);
        }

        private void OnHappinessDown(float value)
        {
            SliderRefresh(_itemSystem.TotalCatHappiness);
        }
    }
}
