using Assets.Scripts.EventBus;
using Assets.Scripts.Utility;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private GameObject[] _levelPrefabs;

  private int _overallLevelIndex = 0;
  private int _currentLevelIndex = 0;
  private GameObject _currentLevelInstance;

  private EventBinding<EventStructs.UIButtonPressed> _uiButtonPressed;

  private void Awake() {
    LoadSettings();
  }

  private void OnEnable() {
    _uiButtonPressed = new EventBinding<EventStructs.UIButtonPressed>(LoadNextLevel);
  }

  private void OnDisable() {
    _uiButtonPressed.Remove(LoadNextLevel);
  }

  private void Start() {
    LoadLevel(_currentLevelIndex);
  }

  public void LoadLevel(int levelIndex) {
    if (_levelPrefabs.Length == 1) {
      levelIndex = 0;
    }
    else if (levelIndex >= _levelPrefabs.Length) {
      levelIndex = Random.Range(1, _levelPrefabs.Length);
    }

    if (levelIndex < _levelPrefabs.Length) {
      if (_currentLevelInstance != null)
        Destroy(_currentLevelInstance);

      _currentLevelInstance = Instantiate(_levelPrefabs[levelIndex]);
    }
  }

  private void LoadNextLevel(EventStructs.UIButtonPressed uiButtonEvent) {
    if (uiButtonEvent.ButtonType != Assets.Scripts.Enums.UIButtonType.NextLevel) return;

    _overallLevelIndex++;
    _currentLevelIndex++;

    SaveSettings();

    if (_currentLevelIndex >= _levelPrefabs.Length) {
      _currentLevelIndex = _levelPrefabs.Length == 1 ? 0 : Random.Range(0, _levelPrefabs.Length);
    }

    LoadLevel(_currentLevelIndex);
  }

  private void LoadSettings() {
    _overallLevelIndex = ES3.Load(SettingsHashes.OverallLevelIndex, 0);
    _currentLevelIndex = ES3.Load(SettingsHashes.LevelIndex, 0);
  }

  private void SaveSettings() {
    ES3.Save(SettingsHashes.OverallLevelIndex, _overallLevelIndex);
    ES3.Save(SettingsHashes.LevelIndex, _currentLevelIndex);
  }
}