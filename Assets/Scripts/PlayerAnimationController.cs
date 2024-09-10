using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class PlayerAnimationController : MonoBehaviour
{
    private const string IS_WALKING_FORWARD = "isWalkingForward";
    private const string IS_WALKING_FORWARD_LEFT = "isWalkingForwardLeft";
    private const string IS_WALKING_FORWARD_RIGHT = "isWalkingForwardRight";
    private const string IS_WALKING_BACKWARD = "isWalkingBackward";
    private const string IS_WALKING_LEFT = "isWalkingLeft";
    private const string IS_WALKING_RIGHT = "isWalkingRight";
    private const string JUMP_TRIGGER = "jumpTrigger";
    private const string IS_RUNNING = "isRunning";

    private Animator _animator;
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputMoveVector;
    private bool _isRunning;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        PlayerMovement.Instance.OnJump += Player_OnJump;

        _playerInputActions.Player.Run.started += OnRunStarted;
        _playerInputActions.Player.Run.canceled += OnRunCanceled;
    }

    private void Player_OnJump(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(JUMP_TRIGGER);
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
        _inputMoveVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        _animator.SetBool(IS_WALKING_FORWARD_LEFT, ((_inputMoveVector.y > 0 && _inputMoveVector.y < 1) && (_inputMoveVector.x < 0 && _inputMoveVector.x > -1)));
        _animator.SetBool(IS_WALKING_FORWARD_RIGHT, ((_inputMoveVector.y > 0 && _inputMoveVector.y < 1) && (_inputMoveVector.x > 0 && _inputMoveVector.x < 1)));

        _animator.SetBool(IS_WALKING_FORWARD, _inputMoveVector.y == 1);
        _animator.SetBool(IS_WALKING_BACKWARD, _inputMoveVector.y < 0);
        _animator.SetBool(IS_WALKING_LEFT, _inputMoveVector.x == -1);
        _animator.SetBool(IS_WALKING_RIGHT, _inputMoveVector.x == 1);

        _animator.SetBool(IS_RUNNING, _isRunning);
    }
}
