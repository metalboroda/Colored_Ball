using Assets.Scripts.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class LoseCanvas : CanvasBase
  {
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start() {
      SubscribeButtons();
    }

    protected override void SubscribeButtons() {
      _restartButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.Restart
        });
      });

      _mainMenuButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.MainMenu
        });
      });
    }
  }
}