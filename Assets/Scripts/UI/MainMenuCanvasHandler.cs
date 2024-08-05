using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure.GameStates;
using Assets.Scripts.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
  public class MainMenuCanvasHandler : MonoBehaviour
  {
    [Header("Canvases")]
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _shopCanvas;

    private List<GameObject> _canvases = new();

    private CanvasChangerService _canvasChangerService;

    private EventBinding<EventStructs.StateChanged> _stateChangedEvent;

    private void Awake() {
      AddCanvasesToList();

      _canvasChangerService = new(_canvases);
    }

    private void OnEnable() {
      _stateChangedEvent = new EventBinding<EventStructs.StateChanged>(OnGameStateChanged);
    }

    private void OnDisable() {
      _stateChangedEvent.Remove(OnGameStateChanged);
    }

    private void AddCanvasesToList() {
      _canvases.Add(_mainMenuCanvas);
      _canvases.Add(_shopCanvas);
    }

    private void OnGameStateChanged(EventStructs.StateChanged stateChanged) {
      switch (stateChanged.State) {
        case GameMainMenuState:
          _canvasChangerService.ChangeCanvas(_mainMenuCanvas);
          break;
        case GameShopState:
          _canvasChangerService.ChangeCanvas(_shopCanvas);
          break;
      }
    }
  }
}