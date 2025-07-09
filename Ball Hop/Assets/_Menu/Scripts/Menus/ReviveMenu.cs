using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sans.UI.Menu
{
    public class ReviveMenu : Menu
    {
        [Header("UI References :")]
        [SerializeField] Button _continueButton;
        [SerializeField] Button _skipButton;

        [Space]
        [SerializeField] TMP_Text _timerText;
        [SerializeField] Image _timerFill;

        private Timer _timer;

        protected override void Awake()
        {
            base.Awake();
            _timer = GetComponent<Timer>();
        }

        private void OnEnable()
        {
            _continueButton.interactable = true;
            StartTimer();

            LeanTween.scale(_continueButton.gameObject, Vector2.one * 1.1f, .3f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong();
        }

        private void Start()
        {
            OnButtonPressed(_continueButton, ContinueButtonPressed);
            OnButtonPressed(_skipButton, SkipButtonPressed);
        }

        private void SkipButtonPressed()
        {
            _timer.StopTimer();
            _menuController.SwitchMenu(MenuType.GameOver);
        }

        private void ContinueButtonPressed()
        {
            _continueButton.interactable = false;

            //Show Rewarded Ad.
            Admob.Instance.ShowRewardAd();
        }

        private void StartTimer()
        {
            StartCoroutine(_timer.CalculateTimer(i => _timerText.text = i, j => _timerFill.fillAmount = j, () =>
            {
                _menuController.SwitchMenu(MenuType.GameOver);
            }));
        }

        private void OnDisable()
        {
            LeanTween.cancel(_continueButton.gameObject);
            _continueButton.transform.localScale = Vector3.one;
        }
    }
}