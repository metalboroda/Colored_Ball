using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
  public static AudioSettingsManager Instance { get; private set; }

  [SerializeField] private AudioMixer _mixer;

  private GameSettings _gameSettings;

  private EventBinding<EventStructs.AudioSwitchedEvent> _audioSwitchedEvent;

  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
    }
    else {
      Instance = this;

      DontDestroyOnLoad(gameObject);
    }
  }

  private void OnEnable() {
    _audioSwitchedEvent = new EventBinding<EventStructs.AudioSwitchedEvent>(SwitchMasterVolume);
  }

  private void OnDisable() {
    _audioSwitchedEvent?.Remove(SwitchMasterVolume);
  }

  private void Start() {
    LoadSettings();
    LoadVolumes();
  }

  private void LoadSettings() {
    _gameSettings = SettingsManager.LoadSettings<GameSettings>();

    if (_gameSettings == null)
      _gameSettings = new GameSettings();
  }

  private void LoadVolumes() {
    if (_gameSettings.IsMusicOn == true)
      _mixer.SetFloat(Hashes.MasterVolume, 0);
    else
      _mixer.SetFloat(Hashes.MasterVolume, -80f);
  }

  public void SwitchMasterVolume() {
    _mixer.GetFloat(Hashes.MasterVolume, out float currentVolume);

    if (currentVolume == 0) {
      _mixer.SetFloat(Hashes.MasterVolume, -80f);
      _gameSettings.IsMusicOn = false;
    }
    else {
      _mixer.SetFloat(Hashes.MasterVolume, 0);
      _gameSettings.IsMusicOn = true;
    }

    SettingsManager.SaveSettings(_gameSettings);
  }
}