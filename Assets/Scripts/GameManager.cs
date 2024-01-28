using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            Cursor.visible = false;
        }

        private void OpenPanel()
        {
            _mainMenuButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();

            _mainMenuButton.onClick.AddListener(LoadMainMenu);
            _restartButton.onClick.AddListener(StartOver);
            _uiPanel.alpha = 1;
            _timerText.text = _counter.GetFormattedTime();
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        private void StartOver()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GameOver()
        {
            _title.text = "Defeat";
            _mottoLine.text = "You made the cat cry!";
            
            OpenPanel();
        }

        public void Win()
        {
            _title.text = "Victory";
            _mottoLine.text = "You made the cat laugh! ^w^";

            OpenPanel();
        }
    }
}
