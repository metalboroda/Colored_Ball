using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure.GameStates;
using UnityEngine;

namespace Assets.Scripts.Services
{
  [RequireComponent(typeof(AudioSource))]
  public class AudioPlayer : MonoBehaviour
  {
    public static AudioPlayer Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private AudioClip _clickSound;
    [Header("Jingles")]
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;

    private float _minRandomPitch = 0.95f;
    private float _maxRandomPitch = 1.05f;

    private AudioSource _audioSource;

    private EventBinding<EventStructs.UIButtonPressed> _uiButtonPressedEvent;
    private EventBinding<EventStructs.StateChanged> _stateChangedEvent;

    private void Awake() {
      _audioSource = GetComponent<AudioSource>();

      InitSingleton();
    }

    private void OnEnable() {
      _uiButtonPressedEvent = new EventBinding<EventStructs.UIButtonPressed>(OnUIButtonPressed);
      _stateChangedEvent = new EventBinding<EventStructs.StateChanged>(OnStateChanged);
    }

    private void OnDisable() {
      if (_uiButtonPressedEvent != null)
        _uiButtonPressedEvent.Remove(OnUIButtonPressed);

      if( _stateChangedEvent != null)
        _stateChangedEvent.Remove(OnStateChanged);
    }

    private void InitSingleton() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
      }
      else {
        Instance = this;

        DontDestroyOnLoad(gameObject);
      }
    }

    private void OnUIButtonPressed(EventStructs.UIButtonPressed uiButtonPressed) {
      PlayWithRandomPitch(_clickSound);
    }

    private void OnStateChanged(EventStructs.StateChanged stateChanged) {
      if (stateChanged.State is GameWinState)
        PlayWithRandomPitch(_winSound);
      else if (stateChanged.State is GameLoseState)
        PlayWithRandomPitch(_loseSound);
    }

    private void PlayWithRandomPitch(AudioClip audioClip) {
      _audioSource.pitch = Random.Range(_minRandomPitch, _maxRandomPitch);

      _audioSource.PlayOneShot(audioClip);
    }
  }
}