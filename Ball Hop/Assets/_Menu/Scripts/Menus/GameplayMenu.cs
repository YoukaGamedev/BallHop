using TMPro;
using UnityEngine;

namespace Sans.UI.Menu
{
    public class GameplayMenu : Menu
    {
        [Header("UI References :")]
        [SerializeField] private TMP_Text _scoreText;

        private void OnEnable()
        {
            ScoreManager sm = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
            if (sm) UpdateScore(sm.Score);
        }

        private void UpdateScore(int currentScore)
        {
            _scoreText.text = currentScore.ToString();
        }

        ////////////////////
        protected override void Awake()
        {
            base.Awake();
            ScoreManager.OnScoreAdded += UpdateScore;
        }

        private void OnDestroy()
        {
            ScoreManager.OnScoreAdded -= UpdateScore;
        }
    }
}