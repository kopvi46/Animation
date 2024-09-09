using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private const string IS_WALKING_FORWARD = "isWalkingForward";
    private const string IS_WALKING_FORWARD_LEFT = "isWalkingForwardLeft";
    private const string IS_WALKING_FORWARD_RIGHT = "isWalkingForwardRight";
    private const string IS_WALKING_BACKWARD = "isWalkingBackward";
    private const string IS_WALKING_LEFT = "isWalkingLeft";
    private const string IS_WALKING_RIGHT = "isWalkingRight";
    private const string IS_JUMPING = "isJumping";
    private const string IS_RUNNING = "isRunning";

    private Animator _animator;
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputVector;
    private bool _isJumping;
    private bool _isRunning;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Jump.started += OnJumpStarted;
        _playerInputActions.Player.Jump.canceled += OnJumpCanceled;
        _playerInputActions.Player.Run.started += OnRunStarted;
        _playerInputActions.Player.Run.canceled += OnRunCanceled;
    }

    private void OnJumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isJumping = false;
    }

    private void OnJumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isJumping = true;
    }

    private void OnRunCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isRunning = false;
    }

    private void OnRunStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isRunning = true;
    }

    private void Update()
    {
        _inputVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        _animator.SetBool(IS_WALKING_FORWARD_LEFT, ((_inputVector.y > 0 && _inputVector.y < 1) && (_inputVector.x < 0 && _inputVector.x > -1)));
        _animator.SetBool(IS_WALKING_FORWARD_RIGHT, ((_inputVector.y > 0 && _inputVector.y < 1) && (_inputVector.x > 0 && _inputVector.x < 1)));

        _animator.SetBool(IS_WALKING_FORWARD, _inputVector.y == 1);
        _animator.SetBool(IS_WALKING_BACKWARD, _inputVector.y < 0);
        _animator.SetBool(IS_WALKING_LEFT, _inputVector.x == -1);
        _animator.SetBool(IS_WALKING_RIGHT, _inputVector.x == 1);

        _animator.SetBool(IS_JUMPING, _isJumping);
        _animator.SetBool(IS_RUNNING, _isRunning);
    }

    private void ResetAllAnimations()
    {
        _animator.SetBool(IS_WALKING_FORWARD_LEFT, false);
        _animator.SetBool(IS_WALKING_FORWARD_RIGHT, false);
        _animator.SetBool(IS_WALKING_FORWARD, false);
        _animator.SetBool(IS_WALKING_BACKWARD, false);
        _animator.SetBool(IS_WALKING_LEFT, false);
        _animator.SetBool(IS_WALKING_RIGHT, false);
    }
}
