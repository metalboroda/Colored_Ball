using Assets.Scripts.EventBus;
using Assets.Scripts.Infrastructure.GameStates;
using Assets.Scripts.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
  public class GameCanvasHandler : MonoBehaviour
  {
    [Header("Canvases")]
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private GameObject _loseCanvas;

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
      _canvases.Add(_gameCanvas);
      _canvases.Add(_pauseCanvas);
      _canvases.Add(_winCanvas);
      _canvases.Add(_loseCanvas);
    }

    private void OnGameStateChanged(EventStructs.StateChanged stateChanged) {
      switch (stateChanged.State) {
        case GamePlayState:
          _canvasChangerService.ChangeCanvas(_gameCanvas);
          break;
        case GamePauseState:
          _canvasChangerService.ChangeCanvas(_pauseCanvas);
          break;
        case GameWinState:
          _canvasChangerService.ChangeCanvas(_winCanvas);
          break;
        case GameLoseState:
          _canvasChangerService.ChangeCanvas(_loseCanvas);
          break;
      }
    }
  }
}