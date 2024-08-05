using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure;
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

    private GameSettings _gameSettings;

    private void Awake() {
      _gameSettings = new GameSettings();

      LoadSettings();
    }

    private void OnDisable() {
      SaveSettings();
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
      _gameSettings.IsAudioOn = !_gameSettings.IsAudioOn;

      UpdateAudioButtonVisuals();
      SaveSettings();

      EventBus<EventStructs.AudioSwitchedEvent>.Raise();
    }

    private void UpdateAudioButtonVisuals() {
      _audioOnIcon.SetActive(_gameSettings.IsAudioOn);
      _audioOffIcon.SetActive(!_gameSettings.IsAudioOn);
    }

    private void LoadSettings() {
      _gameSettings = SettingsManager.LoadSettings<GameSettings>();

      if (_gameSettings == null)
        _gameSettings = new GameSettings();
    }

    private void SaveSettings() {
      SettingsManager.SaveSettings(_gameSettings);
    }
  }
}