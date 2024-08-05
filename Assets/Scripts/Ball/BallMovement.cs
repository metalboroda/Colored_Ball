using Assets.Scripts.EventBus;
using Assets.Scripts.Grid;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Infrastructure.GameStates;
using Assets.Scripts.Memento;
using Assets.Scripts.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Ball
{
  public class BallMovement : MonoBehaviour
  {
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private float _idleDuration = 3f;
    [Space]
    [SerializeField] private GameObject _model;

    private PlayerInputService _playerInputService;
    private BallMovementService _ballMovementService;

    private Dictionary<Vector2, Vector3> _directionMap;
    private Stack<GameMemento> _mementos;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<EventStructs.UIButtonPressed> _undoButtonPressedEvent;

    private void Awake() {
      _playerInputService = new PlayerInputService();
      _mementos = new Stack<GameMemento>();

      _directionMap = new Dictionary<Vector2, Vector3>
      {
        { Vector2.up, Vector3.forward },
        { Vector2.down, Vector3.back },
        { Vector2.left, Vector3.left },
        { Vector2.right, Vector3.right }
      };

      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable() {
      _undoButtonPressedEvent = new EventBinding<EventStructs.UIButtonPressed>(UndoLastMove);
    }

    private void OnDisable() {
      _undoButtonPressedEvent.Remove(UndoLastMove);
    }

    private void Update() {
      if (_gameBootstrapper.FiniteStateMachine.CurrentState is not GamePlayState) return;

      Vector2 inputDirection = _playerInputService.GetMovementDirection();

      if (_directionMap.ContainsKey(inputDirection)) {
        SaveMemento();

        Vector3 direction = _directionMap[inputDirection];
        _ballMovementService.MoveBall(direction);
      }
    }

    public void SpawnInit(float movementSpeed, GridManager gridManager) {
      _ballMovementService = new BallMovementService(
          movementSpeed, gridManager, transform, _moveDuration, _model, _idleDuration);
    }

    private void SaveMemento() {
      _mementos.Push(_ballMovementService.CreateMemento());
    }

    private void UndoLastMove(EventStructs.UIButtonPressed uiButtonPressed) {
      if (uiButtonPressed.ButtonType != Enums.UIButtonType.Undo) return;

      if (_mementos.Count > 0) {
        GameMemento memento = _mementos.Pop();

        _ballMovementService.RestoreMemento(memento);
      }
    }
  }
}