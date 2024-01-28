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
            // Cursor.visible = false;
        }

        private void OpenPanel()
        {
            var itmsys = FindObjectOfType<ItemSystem>();
            if (itmsys != null)
            {
                Destroy(itmsys.gameObject);
            }
            Time.timeScale = 0;
            _mainMenuButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();

            _mainMenuButton.onClick.AddListener(LoadMainMenu);
            _restartButton.onClick.AddListener(StartOver);
            _uiPanel.alpha = 1;
            _timerText.text = _counter.GetFormattedTime();
        }

        private void LoadMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenuScene");
        }

        private void StartOver()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
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
