using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sans.UI.Menu
{
    public class GameoverMenu : Menu
    {
        [Header("UI References :")]
        [SerializeField] TMP_Text _scoreText;
        [SerializeField] TMP_Text _bestScoreText;
        [SerializeField] Button _restartButton;
        [SerializeField] Button _homeButton;
        [SerializeField] Button _shareButton;

        ShareOnSocialMedia _share;

        protected override void Awake()
        {
            base.Awake();
            _share = GetComponent<ShareOnSocialMedia>();
        }

        private void OnEnable()
        {
            _restartButton.interactable = true;
            _homeButton.interactable = true;

            ScoreManager[] scArray = FindObjectsByType<ScoreManager>(FindObjectsSortMode.None);

            if (scArray.Length > 0 && scArray[0] != null)
            {
                ScoreManager sc = scArray[0];
                int lastScore = sc.Score;
                int bestScore = sc.GetBestScore();
                SetScoreDisplay(lastScore, bestScore);
            }
            else
            {
                Debug.LogWarning("⚠ ScoreManager tidak ditemukan di scene.");
                SetScoreDisplay(0, 0); // default score jika tidak ada
            }
        }

        private void Start()
        {
            OnButtonPressed(_restartButton, RestartButton);
            OnButtonPressed(_homeButton, HomeButton);
            OnButtonPressed(_shareButton, HandleShareButtonPressed);
        }

        private void SetScoreDisplay(int lastScore, int bestScore)
        {
            _scoreText.text = $"Last : {lastScore}";
            _bestScoreText.text = $"Best : {bestScore}";
        }

        private void HomeButton()
        {
            _restartButton.interactable = false;

            StartCoroutine(ReloadLevelAsync(() =>
            {
                _menuController.SwitchMenu(MenuType.Main);
            }));
        }

        private void HandleShareButtonPressed()
        {
            _share.HandleShare();
        }

        private void RestartButton()
        {
            _homeButton.interactable = false;

            StartCoroutine(ReloadLevelAsync(() =>
            {
                _menuController.SwitchMenu(MenuType.Gameplay);
            }));
        }

        IEnumerator ReloadLevelAsync(Action OnSceneLoaded = null)
        {
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            OnSceneLoaded?.Invoke();
        }
    }
}
