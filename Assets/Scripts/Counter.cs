using System;
using UnityEngine;
using UnityEngine.UI;

namespace KrakJam2024
{
    public class Counter : MonoBehaviour
    {
        public string GetFormattedTime() => FormatTime(Time.time);

        private string FormatTime(float time)
        {
            int intTime = (int)time;
            int minutes = intTime / 60;
            int seconds = intTime % 60;
            float fraction = time * 1000;
            fraction = (fraction % 1000);
            string timeText = String.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, fraction);
            return timeText;
        }
    }
}
