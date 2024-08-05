using Assets.Scripts.EventBus;
using Assets.Scripts.Grid;
using Assets.Scripts.Infrastructure.GameStates;
using Assets.Scripts.Memento;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementService
{
  private float _movementSpeed;
  private Transform _transform;
  private float _moveDuration;
  private bool _isMoving;
  private GameObject _model;
  private Vector3 _currentDirection;
  private Vector3 _newDirection;
  private bool _directionChanged;
  private float _idleTimer;
  private bool _hasMoved;
  private Coroutine _idleCoroutine;

  private GridManager _gridManager;

  public bool IsMoving => _isMoving;

  private EventBinding<EventStructs.StateChanged> _stateChanged;

  public BallMovementService(float movementSpeed,
    GridManager gridManager, Transform transform,
    float moveDuration, GameObject model, float idleTimer) {
    _movementSpeed = movementSpeed;
    _transform = transform;
    _moveDuration = moveDuration;
    _model = model;
    _directionChanged = false;
    _idleTimer = idleTimer;
    _hasMoved = false;

    _gridManager = gridManager;

    StopIdleCoroutine();
  }

  public void MoveBall(Vector3 direction) {
    if (!_isMoving) {
      _currentDirection = direction;
      _newDirection = direction;
      _directionChanged = false;

      RotateModel(direction);

      _isMoving = true;

      StopIdleCoroutine();

      _hasMoved = true;

      ContinueMoving();
    }
    else {
      _newDirection = direction;
      _directionChanged = true;
    }
  }

  private void ContinueMoving() {
    if (!_isMoving) return;

    Vector3 targetPosition = SnapToGrid(_transform.position + _currentDirection);

    if (_gridManager.IsPositionWithinBounds(targetPosition) && !_gridManager.IsTilePainted(targetPosition)) {
      _transform.DOMove(targetPosition, _moveDuration).OnComplete(() => {
        _transform.position = targetPosition;
        _gridManager.PaintTileAtPosition(targetPosition);

        if (_directionChanged && HasValidMovesInDirection(_newDirection)) {
          _currentDirection = _newDirection;
          _directionChanged = false;

          RotateModel(_newDirection);
        }

        if (HasValidMovesInDirection(_currentDirection)) {
          ContinueMoving();
        }
        else {
          _isMoving = false;

          StartIdleCoroutine();
        }
      });
    }
    else {
      _isMoving = false;

      StartIdleCoroutine();
    }
  }

  private bool HasValidMovesInDirection(Vector3 direction) {
    Vector3 checkPos = SnapToGrid(_transform.position + direction);
    return _gridManager.IsPositionWithinBounds(checkPos) && !_gridManager.IsTilePainted(checkPos);
  }

  private void LogNoValidMoves() {
    EventBus<EventStructs.NowhereToGo>.Raise(new EventStructs.NowhereToGo());
  }

  public GameMemento CreateMemento() {
    List<Vector3> paintedTilesPositions;
    var tileMementos = _gridManager.CreateMemento(out paintedTilesPositions);

    return new GameMemento(SnapToGrid(_transform.position), tileMementos, paintedTilesPositions);
  }

  public void RestoreMemento(GameMemento memento) {
    if (_isMoving) return;

    _transform.position = SnapToGrid(memento.BallPosition);
    _gridManager.RestoreMemento(memento.TileStates, memento.PaintedTilesPositions);
    _gridManager.PaintTileAtPosition(memento.BallPosition);

    StopIdleCoroutine();
  }

  private Vector3 SnapToGrid(Vector3 position) {
    int x = Mathf.RoundToInt(position.x);
    int z = Mathf.RoundToInt(position.z);

    return new Vector3(x, position.y, z);
  }

  private void RotateModel(Vector3 direction) {
    if (_model != null) {
      Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

      _model.transform.DORotateQuaternion(targetRotation, _moveDuration / 2);
    }
  }

  private void StartIdleCoroutine() {
    if (_hasMoved && _idleCoroutine == null) {
      _idleCoroutine = _transform.GetComponent<MonoBehaviour>().StartCoroutine(IdleCountdown());
    }
  }

  private void StopIdleCoroutine() {
    if (_idleCoroutine != null) {
      _transform.GetComponent<MonoBehaviour>().StopCoroutine(_idleCoroutine);

      _idleCoroutine = null;
    }
  }

  private IEnumerator IdleCountdown() {
    yield return new WaitForSeconds(_idleTimer);

    LogNoValidMoves();
  }
}