using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class GameCanvas : CanvasBase
  {
    [SerializeField] private Button _undoButton;
    [SerializeField] private Button _pauseButton;
    [Space]
    [SerializeField] private TextMeshProUGUI _levelCounterText;
    [SerializeField] private TextMeshProUGUI _coinCounterText;

    private GameSettings _gameSettings;

    private EventBinding<EventStructs.UpdateCoinAmount> _updateCoinAmountEvent;

    private void Awake() {
      LoadSettings();
    }

    private void OnEnable() {
      _updateCoinAmountEvent = new EventBinding<EventStructs.UpdateCoinAmount>(UpdateCoinAmountText);
    }

    private void OnDisable() {
      _updateCoinAmountEvent.Remove(UpdateCoinAmountText);
    }

    private void Start() {
      SubscribeButtons();
    }

    protected override void SubscribeButtons() {
      _undoButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Undo
        });
      });

      _pauseButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Pause
        });
      });
    }

    private void UpdateCoinAmountText(EventStructs.UpdateCoinAmount updateCoinAmount) {
      _coinCounterText.text = updateCoinAmount.Amount.ToString();
    }

    private void LoadSettings() {
      _gameSettings = SettingsManager.LoadSettings<GameSettings>();

      if (_gameSettings == null)
        _gameSettings = new GameSettings();

      _levelCounterText.text = $"Level: {_gameSettings.OverallLevelIndex + 1}";
    }
  }
}