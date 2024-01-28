using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KrakJam2024
{
    public class PercentageDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private Text _text;

        private void Awake()
        {
            _slider.onValueChanged.RemoveAllListeners();
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            _text.text = $"{(int)(((value / _slider.maxValue) * 100))}%";
        }
    }
}
