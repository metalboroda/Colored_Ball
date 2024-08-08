using Assets.Scripts.EventBus;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class PauseCanvas : CanvasBase
  {
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _audioButton;
    [SerializeField] private GameObject _audioOnIcon;
    [SerializeField] private GameObject _audioOffIcon;

    private bool _isAudioOn;

    private void Awake() {
      LoadSettings();
    }

    private void Start() {
      SubscribeButtons();
      UpdateAudioButtonVisuals();
    }

    protected override void SubscribeButtons() {
      _continueButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Continue
        });
      });

      _restartButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Restart
        });
      });

      _mainMenuButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.MainMenu
        });
      });

      _audioButton.onClick.AddListener(SwitchAudioVolumeButton);
    }

    private void SwitchAudioVolumeButton() {
      _isAudioOn = !_isAudioOn;

      UpdateAudioButtonVisuals();
      SaveSettings();

      EventBus<EventStructs.AudioSwitchedEvent>.Raise();
    }

    private void UpdateAudioButtonVisuals() {
      _audioOnIcon.SetActive(_isAudioOn);
      _audioOffIcon.SetActive(!_isAudioOn);
    }

    private void LoadSettings() {
      _isAudioOn = ES3.Load(SettingsHashes.IsAudioOn, _isAudioOn = true);
    }

    private void SaveSettings() {
      ES3.Save(SettingsHashes.IsAudioOn, _isAudioOn);
    }
  }
}