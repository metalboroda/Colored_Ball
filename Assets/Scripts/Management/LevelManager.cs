using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private GameObject[] _levelPrefabs;

  private int _overallLevelIndex = 0;
  private int _currentLevelIndex = 0;
  private GameObject _currentLevelInstance;

  private GameSettings _gameSettings;

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
    SetSavedLevel();
    LoadLevel(_currentLevelIndex);
  }

  private void OnDestroy() {
    SettingsManager.SaveSettings(_gameSettings);
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
    _gameSettings.OverallLevelIndex = _overallLevelIndex;
    _currentLevelIndex++;
    _gameSettings.LevelIndex = _currentLevelIndex;

    if (_currentLevelIndex >= _levelPrefabs.Length) {
      _currentLevelIndex = _levelPrefabs.Length == 1 ? 0 : Random.Range(0, _levelPrefabs.Length);
    }

    SettingsManager.SaveSettings(_gameSettings);
    LoadLevel(_currentLevelIndex);
  }

  private void LoadSettings() {
    _gameSettings = SettingsManager.LoadSettings<GameSettings>();

    if (_gameSettings == null)
      _gameSettings = new GameSettings();
  }

  private void SetSavedLevel() {
    _currentLevelIndex = _gameSettings.LevelIndex;
    _overallLevelIndex = _gameSettings.OverallLevelIndex;
  }
}