using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Audio
{
  [RequireComponent(typeof(AudioSource))]
  public class Soundtrack : MonoBehaviour
  {
    public static Soundtrack Instance { get; private set; }

    [SerializeField] private AudioMixer _musicMixer;

    [Space]
    [SerializeField] private AudioClip[] _soundtrackClips;

    private readonly List<int> _previousTracks = new List<int>();
    private bool _isPaused = false;

    private AudioSource _audioSource;
    private Coroutine _playSoundtracksCoroutine;

    private void Awake() {
      _audioSource = GetComponent<AudioSource>();

      if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _playSoundtracksCoroutine = StartCoroutine(DoPlaySoundtracks());
      }
      else {
        Destroy(gameObject);
      }
    }

    private void OnDestroy() {
      if (Instance == this) {
        Instance = null;
      }
    }

    private IEnumerator DoPlaySoundtracks() {
      while (true) {
        if (!_isPaused) {
          int randomIndex = GetRandomTrackIndex();

          _audioSource.clip = _soundtrackClips[randomIndex];
          _audioSource.Play();

          while (_audioSource.isPlaying) {
            yield return null;
          }

          _previousTracks.Add(randomIndex);

          if (_previousTracks.Count > 2) {
            _previousTracks.RemoveAt(0);
          }
        }
        else {
          yield return new WaitForSeconds(0.1f);
        }
      }
    }

    private int GetRandomTrackIndex() {
      int randomIndex;

      do {
        randomIndex = Random.Range(0, _soundtrackClips.Length);
      } while (_previousTracks.Contains(randomIndex));

      return randomIndex;
    }

    private void OnApplicationPause(bool pauseStatus) {
      if (pauseStatus) {
        _audioSource.Pause();
        _isPaused = true;
      }
      else {
        _audioSource.UnPause();
        _isPaused = false;
      }
    }
  }
}