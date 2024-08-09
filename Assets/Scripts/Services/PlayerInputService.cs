using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Services
{
  public class PlayerInputService
  {
    private PlayerInputActions _playerInputActions;
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private bool _isSwiping;
    private float _minSwipeDistance = 25f;

    public PlayerInputService() {
      _playerInputActions = new PlayerInputActions();

      EnableBasicMap();
    }

    private void EnableBasicMap() {
      _playerInputActions.Basic.Enable();
    }

    public Vector2 GetMovementDirection() {
#if UNITY_ANDROID || UNITY_IOS
      return GetSwipeDirection();
#else
      return _playerInputActions.Basic.Movement.ReadValue<Vector2>();
#endif
    }

    private Vector2 GetSwipeDirection() {
      if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) {
        if (!_isSwiping) {
          _startTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
          _isSwiping = true;
        }
        _endTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
      }
      else {
        if (_isSwiping) {
          _isSwiping = false;

          Vector2 swipeDelta = _endTouchPosition - _startTouchPosition;

          if (swipeDelta.magnitude >= _minSwipeDistance) {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) {
              // Horizontal swipe
              return swipeDelta.x > 0 ? Vector2.right : Vector2.left;
            }
            else {
              // Vertical swipe
              return swipeDelta.y > 0 ? Vector2.up : Vector2.down;
            }
          }
        }
      }

      return Vector2.zero;
    }
  }
}