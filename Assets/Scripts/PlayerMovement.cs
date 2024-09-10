using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public event EventHandler OnJump;

    private const string IDLE_ANIMATION = "Breathing Idle";

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeedMultiplier;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private LayerMask _groundLayer;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private PlayerInputActions _playerInputActions;
    private float _currentRotation;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Jump.performed += Jump;
        _playerInputActions.Player.Run.started += OnRunStarted;
        _playerInputActions.Player.Run.canceled += OnRunCanceled;
    }

    private void OnRunCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _speed = _speed / _runSpeedMultiplier;
    }

    private void OnRunStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _speed = _speed * _runSpeedMultiplier;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector2 inputVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        Vector3 directionVector = cameraForward * inputVector.y + cameraRight * inputVector.x;

        float moveDistance = _speed * Time.deltaTime;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ANIMATION))
        {
            transform.position += directionVector * moveDistance;

        }
    }

    private void HandleRotation()
    {
        Vector2 inputVector = _playerInputActions.Player.Rotation.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        
        float mouseRotation = inputVector.x * _mouseSensitivity * Time.deltaTime;
        _currentRotation = Mathf.Lerp(_currentRotation, mouseRotation, Time.deltaTime * 10f);

        transform.Rotate(Vector3.up * _currentRotation);
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        bool isGrounded;
        float groundDistance = .1f;

        isGrounded = Physics.CheckSphere(transform.position, groundDistance, _groundLayer);

        if (isGrounded && _animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ANIMATION))
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            OnJump?.Invoke(this, EventArgs.Empty);
        }
    }
}
