using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Infrastructure.GameStates;
using UnityEngine;

namespace Assets.Scripts.ShopLogic
{
  public class Shop : MonoBehaviour
  {
    public static Shop Instance { get; private set; }

    [SerializeField] private GameObject _characterContainer;
    [Space]
    [SerializeField] private ShopItem[] _shopItems;

    private int _currentCharacterIndex = 0;
    private GameObject _currentCharacterInstance;
    private string _currentCharacterName;

    private GameSettings _gameSettings;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<EventStructs.StateChanged> _stateChangedEvent;
    private EventBinding<EventStructs.UIButtonPressed> _uiButtonPressedEvent;

    private void Awake() {
      Instance = this;

      _gameBootstrapper = GameBootstrapper.Instance;

      LoadSettings();
    }

    private void OnEnable() {

      _stateChangedEvent = new EventBinding<EventStructs.StateChanged>(OnGameStateChanged);
      _uiButtonPressedEvent = new EventBinding<EventStructs.UIButtonPressed>(OnUIButtonPressed);
    }

    private void OnDisable() {
      _stateChangedEvent.Remove(OnGameStateChanged);
      _uiButtonPressedEvent.Remove(OnUIButtonPressed);
    }

    private void OnDestroy() {
      SaveSettings();
    }

    private void OnGameStateChanged(EventStructs.StateChanged stateChanged) {
      if (stateChanged.State is GameShopState) {

        _characterContainer.SetActive(true);

        if (_shopItems.Length > 0) {
          _currentCharacterIndex = 1;

          SpawnCharacter(_currentCharacterIndex);
        }
      }
      else {
        _characterContainer.SetActive(false);

        DestroyCurrentCharacter();
        SaveSettings();
      }
    }

    private void OnUIButtonPressed(EventStructs.UIButtonPressed uiButtonPressed) {
      switch (uiButtonPressed.ShopButtonType) {
        case Enums.ShopButtonType.Previous:
          ShowPreviousCharacter();
          break;
        case Enums.ShopButtonType.Next:
          ShowNextCharacter();
          break;
        case Enums.ShopButtonType.Buy:
          BuyCurrentCharacter();
          SaveSettings();
          break;
        case Enums.ShopButtonType.Select:
          SelectCurrentCharacter();
          break;
      }
    }

    private void ShowPreviousCharacter() {
      if (_shopItems.Length > 0) {
        _currentCharacterIndex = (_currentCharacterIndex - 1 + _shopItems.Length) % _shopItems.Length;

        SpawnCharacter(_currentCharacterIndex);
      }
    }

    private void ShowNextCharacter() {
      if (_shopItems.Length > 0) {
        _currentCharacterIndex = (_currentCharacterIndex + 1) % _shopItems.Length;

        SpawnCharacter(_currentCharacterIndex);
      }
    }

    private void SpawnCharacter(int index) {
      DestroyCurrentCharacter();

      if (index >= 0 && index < _shopItems.Length) {
        _currentCharacterInstance = Instantiate(_shopItems[index].Prefab, _characterContainer.transform);
      }

      EventBus<EventStructs.ShopCharacterSpawned>.Raise(new EventStructs.ShopCharacterSpawned {
        Name = _currentCharacterInstance.name,
        Price = _shopItems[index].Price,
        Unlocked = _shopItems[index].Unlocked,
      });
    }

    private void DestroyCurrentCharacter() {
      if (_currentCharacterInstance != null) {
        Destroy(_currentCharacterInstance);
      }
    }

    private void BuyCurrentCharacter() {
      if (_shopItems.Length > 0 && !_shopItems[_currentCharacterIndex].Unlocked) {
        if (_gameBootstrapper.MoneyManager.CoinAmount < _shopItems[_currentCharacterIndex].Price) return;

        _gameBootstrapper.MoneyManager.TrySpendMoney(_shopItems[_currentCharacterIndex].Price);

        _shopItems[_currentCharacterIndex].Unlocked = true;

        EventBus<EventStructs.ShopCharacterSpawned>.Raise(new EventStructs.ShopCharacterSpawned {
          Name = _currentCharacterInstance.name,
          Price = _shopItems[_currentCharacterIndex].Price,
          Unlocked = _shopItems[_currentCharacterIndex].Unlocked,
        });

        SelectCurrentCharacter();
      }
    }

    private void SelectCurrentCharacter() {
      if (_shopItems.Length > 0 && _shopItems[_currentCharacterIndex].Unlocked) {
        _currentCharacterName = _currentCharacterInstance.name.Replace("(Clone)", "").Trim();
      }
    }

    private void SaveSettings() {
      _gameSettings.UnlockedCharacters.Clear();

      foreach (var item in _shopItems) {
        _gameSettings.UnlockedCharacters.Add(item.Unlocked);
      }

      _gameSettings.BallName = _currentCharacterName;

      SettingsManager.SaveSettings(_gameSettings);
    }

    private void LoadSettings() {
      _gameSettings = SettingsManager.LoadSettings<GameSettings>();

      if (_gameSettings == null)
        _gameSettings = new GameSettings();

      if (_gameSettings != null) {
        for (int i = 0; i < _shopItems.Length; i++) {
          if (i < _gameSettings.UnlockedCharacters.Count) {
            _shopItems[i].Unlocked = _gameSettings.UnlockedCharacters[i];
          }
        }
      }

      _currentCharacterName = _gameSettings.BallName;
    }
  }
}