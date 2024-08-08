using Assets.Scripts.EventBus;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Management
{
  public class AudioSettingsManager : MonoBehaviour
  {
    public static AudioSettingsManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mixer;

    private bool _isAudioOn;

    private EventBinding<EventStructs.AudioSwitchedEvent> _audioSwitchedEvent;

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
      }
      else {
        Instance = this;

        DontDestroyOnLoad(gameObject);
      }

      LoadSettings();
    }

    private void OnEnable() {
      _audioSwitchedEvent = new EventBinding<EventStructs.AudioSwitchedEvent>(SwitchMasterVolume);
    }

    private void OnDisable() {
      _audioSwitchedEvent?.Remove(SwitchMasterVolume);
    }

    private void Start() {
      LoadVolumes();
    }

    private void LoadVolumes() {
      if (_isAudioOn == true)
        _mixer.SetFloat(SettingsHashes.MasterVolume, 0);
      else
        _mixer.SetFloat(SettingsHashes.MasterVolume, -80f);
    }

    public void SwitchMasterVolume() {
      _mixer.GetFloat(SettingsHashes.MasterVolume, out float currentVolume);

      if (currentVolume == 0) {
        _mixer.SetFloat(SettingsHashes.MasterVolume, -80f);

        _isAudioOn = false;
      }
      else {
        _mixer.SetFloat(SettingsHashes.MasterVolume, 0);

        _isAudioOn = true;
      }

      SaveSettings();
    }

    private void LoadSettings() {
      _isAudioOn = ES3.Load(SettingsHashes.IsAudioOn, _isAudioOn = true);
    }

    private void SaveSettings() {
      ES3.Save(SettingsHashes.IsAudioOn, _isAudioOn);
    }
  }
}