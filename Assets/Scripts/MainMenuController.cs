using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KrakJam2024
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Button _startGame;

        [SerializeField]
        private Button _quitGame;

        private void Awake()
        {
            _startGame.onClick.RemoveAllListeners();
            _quitGame.onClick.RemoveAllListeners();

            _startGame.onClick.AddListener(StartGame);
            _quitGame.onClick.AddListener(QuitGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        private void QuitGame()
        {
            Application.Quit(0);
        }
    }
}
