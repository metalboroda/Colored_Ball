using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure.GameStates;
using Assets.Scripts.Infrastructure;
using UnityEngine;
using System.Collections;

public class MoneyManager
{
  public int CoinAmount { get; set; }

  private Coroutine _eventCoroutine;

  private MonoBehaviour _monoBehaviour;
  private GameSettings _gameSettings;

  private EventBinding<EventStructs.CoinPickedUp> _coinPickedUpEvent;
  private EventBinding<EventStructs.StateChanged> _stateChangeEvent;

  public MoneyManager(MonoBehaviour monoBehaviour) {
    _monoBehaviour = monoBehaviour;

    _coinPickedUpEvent = new EventBinding<EventStructs.CoinPickedUp>(OnCoinPickedUp);
    _stateChangeEvent = new EventBinding<EventStructs.StateChanged>(OnStateChangeEvent);

    LoadSettings();
    LoadCoinAmount();
  }

  public void TrySpendMoney(int amount) {
    CoinAmount -= amount;

    SaveCoinAmount();
    SendEvents();
  }

  public void Dispose() {
    _coinPickedUpEvent.Remove(OnCoinPickedUp);
    _stateChangeEvent.Remove(OnStateChangeEvent);

    SaveCoinAmount();
  }

  public void SendEvents() {
    _monoBehaviour.StartCoroutine(DoSendEvent());
  }

  private IEnumerator DoSendEvent() {
    yield return new WaitForEndOfFrame();

    EventBus<EventStructs.UpdateCoinAmount>.Raise(new EventStructs.UpdateCoinAmount {
      Amount = CoinAmount
    });
  }

  private void OnCoinPickedUp(EventStructs.CoinPickedUp coinPickedUp) {
    CoinAmount += coinPickedUp.Value;

    SendEvents();
  }

  private void OnStateChangeEvent(EventStructs.StateChanged stateChanged) {
    if (stateChanged.State is GameWinState) {
      SaveCoinAmount();
      SendEvents();
    }
    else {
      LoadCoinAmount();
      SendEvents();
    }
  }

  private void SaveCoinAmount() {
    _gameSettings.CoinAmount = CoinAmount;

    SettingsManager.SaveSettings(_gameSettings);
  }

  private void LoadSettings() {
    _gameSettings = SettingsManager.LoadSettings<GameSettings>();

    if (_gameSettings == null)
      _gameSettings = new GameSettings();
  }

  private void LoadCoinAmount() {
    CoinAmount = _gameSettings.CoinAmount;
  }
}