using Assets.Scripts.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class ShopCanvas : CanvasBase
  {
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _mainMenuButton;
    [Space]
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _coinCounterText;

    private EventBinding<EventStructs.UpdateCoinAmount> _updateCoinAmountEvent;
    private EventBinding<EventStructs.ShopCharacterSpawned> _shopCharacterSpawned;

    private void OnEnable() {
      _updateCoinAmountEvent = new EventBinding<EventStructs.UpdateCoinAmount>(UpdateCoinCounterText);
      _shopCharacterSpawned = new EventBinding<EventStructs.ShopCharacterSpawned>(OnShopCharacterSpawned);
    }

    private void OnDisable() {
      _updateCoinAmountEvent.Remove(UpdateCoinCounterText);
      _shopCharacterSpawned.Remove(OnShopCharacterSpawned);
    }

    private void Start() {
      SubscribeButtons();
    }

    protected override void SubscribeButtons() {
      _mainMenuButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ButtonType = Enums.UIButtonType.MainMenu
        });
      });

      _previousButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ShopButtonType = Enums.ShopButtonType.Previous
        });
      });

      _nextButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ShopButtonType = Enums.ShopButtonType.Next
        });
      });

      _selectButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ShopButtonType = Enums.ShopButtonType.Select
        });
      });

      _buyButton.onClick.AddListener(() => {
        EventBus<EventStructs.UIButtonPressed>.Raise(new EventStructs.UIButtonPressed {
          ShopButtonType = Enums.ShopButtonType.Buy
        });
      });
    }

    private void UpdateCoinCounterText(EventStructs.UpdateCoinAmount updateCoinAmount) {
      _coinCounterText.text = updateCoinAmount.Amount.ToString();
    }

    private void OnShopCharacterSpawned(EventStructs.ShopCharacterSpawned shopCharacterSpawned) {
      _priceText.text = shopCharacterSpawned.Price.ToString();

      if(shopCharacterSpawned.Unlocked == true) {
        _buyButton.gameObject.SetActive(false);
        _selectButton.gameObject.SetActive(true);
        _priceText.gameObject.SetActive(false);
      }
      else {
        _buyButton.gameObject.SetActive(true);
        _selectButton.gameObject.SetActive(false);
        _priceText.gameObject.SetActive(true);
      }
    }
  }
}