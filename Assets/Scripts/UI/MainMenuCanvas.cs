using Assets.Scripts.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class MainMenuCanvas : CanvasBase
  {
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _shopButton;

    private void Start() {
      SubscribeButtons();
    }

    protected override void SubscribeButtons() {
      _startButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Start
        });
      });

      _shopButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Shop
        });
      });
    }
  }
}