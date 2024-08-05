using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class WinCanvas : CanvasBase
  {
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _mainMenuButton;
    [Space]
    [SerializeField] private TextMeshProUGUI _coinCounterText;

    private EventBinding<EventStructs.UpdateCoinAmount> _updateCoinAmount;

    private void OnEnable() {
      _updateCoinAmount = new EventBinding<EventStructs.UpdateCoinAmount>(UpdateCoinAmount);
    }

    private void OnDisable() {
      _updateCoinAmount.Remove(UpdateCoinAmount);
    }

    private void Start() {
      SubscribeButtons();
    }

    protected override void SubscribeButtons() {
      _nextLevelButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = UIButtonType.NextLevel
        });
      });

      _mainMenuButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = UIButtonType.MainMenu
        });
      });
    }

    private void UpdateCoinAmount(EventStructs.UpdateCoinAmount updateCoinAmount) {
      _coinCounterText.text = updateCoinAmount.Amount.ToString();
    }
  }
}