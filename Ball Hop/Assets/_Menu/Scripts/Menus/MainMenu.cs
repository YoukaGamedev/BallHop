using UnityEngine;
using UnityEngine.UI;

namespace Sans.UI.Menu
{
    public class MainMenu : Menu
    {
        [Header("UI References :")]
        [SerializeField] Button _creditButton;
        [SerializeField] Button _rateButton;
        [SerializeField] Button _settingsButton;
        [SerializeField] GameObject _tutorial;

        [Header("Data References :")]
        [SerializeField] MenuData _data;

        private void OnEnable()
        { 
            LeanTween.scale(_tutorial, Vector2.one * 1.05f, .3f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong();
        }

        private void OnDisable()
        {
            LeanTween.cancel(_tutorial);
            _tutorial.transform.localScale = Vector3.one;
        }

        private void Start()
        {
            OnButtonPressed(_creditButton, CreditButtonPressed);
            OnButtonPressed(_rateButton, RateButtonPressed);
            OnButtonPressed(_settingsButton, SettingsButtonPressed);
        }

        private void SettingsButtonPressed()
        {
            _menuController.OpenMenu(MenuType.Setting);
        }

        private void RateButtonPressed()
        {
            Application.OpenURL($"market://details?id={_data.PackageName}");
        }

        private void CreditButtonPressed()
        {
            _menuController.OpenMenu(MenuType.Credit);
        }
    }
}
