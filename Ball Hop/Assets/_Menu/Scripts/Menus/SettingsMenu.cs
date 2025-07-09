using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sans.UI.Menu
{
    public class SettingsMenu : Menu
    {
        [Header("UI References :")]
        [SerializeField] Button _adsButton;
        [SerializeField] Button _privacyButton;
        [SerializeField] Button _backButton;
        [SerializeField] Button _musicButton;
        [SerializeField] Button _sfxButton;

        [Space]
        [SerializeField] int _GDPRSceneIndex = 0;

        [Header("Database References :")]
        [SerializeField] MenuData _data;

        [Header("Image References Toggle")]
        [SerializeField] Image _sfxImage;
        [SerializeField] Image _MusicImage;

        [Header("Icon Toggle")]
        [SerializeField] Sprite _iconTrue;
        [SerializeField] Sprite _iconFalse;

        private bool _sfxState;
        private bool _musicState;

        private void OnEnable()
        {
            _backButton.interactable = true;
        }

        private void Start()
        {
            SetButtonIconToggle();

            OnButtonPressed(_adsButton, AdsButtonPressed);
            OnButtonPressed(_musicButton, MusicButtonPressed);
            OnButtonPressed(_sfxButton, SfxButtonPressed);
            OnButtonPressed(_privacyButton, PrivacyPolicyButtonPressed);
            OnButtonPressed(_backButton, BackButtonPressed);
        }

        private void SetButtonIconToggle()
        {
            _musicState = PlayerPrefs.GetInt("musicState", 0) == 0;
            _sfxState = PlayerPrefs.GetInt("sfxState", 0) == 0;

            ToggleIconSFX();
            ToggleIconMusic();
        }

        private void ToggleIconSFX()
        {
            if (!_sfxImage || !_iconTrue || !_iconFalse) return;

            _sfxImage.sprite = _sfxState ? _iconTrue : _iconFalse;
        }

        private void ToggleIconMusic()
        {
            if (!_MusicImage || !_iconTrue || !_iconFalse) return;

            _MusicImage.sprite = _musicState ? _iconTrue : _iconFalse;
        }

        private void SfxButtonPressed()
        {
            SoundController.Instance.ToggleFX(ref _sfxState);
            ToggleIconSFX();
        }

        private void MusicButtonPressed()
        {
            SoundController.Instance.ToggleMusic(ref _musicState);
            ToggleIconMusic();
        }

        private void BackButtonPressed()
        {
            _backButton.interactable = false;
            _menuController.CloseMenu();
        }

        private void PrivacyPolicyButtonPressed()
        {
            Application.OpenURL(_data.PrivacyPolicy);
        }

        private void AdsButtonPressed()
        {
            PlayerPrefs.SetInt("npa", -1);

            //load gdpr scene
            SceneManager.LoadScene(_GDPRSceneIndex);
        }
    }
}
