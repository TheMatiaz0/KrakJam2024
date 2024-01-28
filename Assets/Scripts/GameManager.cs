using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KrakJam2024
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _uiPanel;
        [SerializeField]
        private Button _mainMenuButton;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Counter _counter;
        [SerializeField]
        private Text _timerText;
        [SerializeField]
        private Text _mottoLine;
        [SerializeField]
        private Text _title;

        private void Awake()
        {
            _uiPanel.alpha = 0;
        }

        private void OpenPanel()
        {
            _uiPanel.alpha = 1;
            _timerText.text = _counter.CounterText.text;
        }

        public void GameOver()
        {
            _title.text = "Defeat";
            _mottoLine.text = "You made the cat cry! Are you proud of yourself?";
            
            OpenPanel();
        }

        public void Win()
        {
            _title.text = "Victory";
            _mottoLine.text = "You made the cat laugh! You are a great chef!";

            OpenPanel();
        }
    }
}
