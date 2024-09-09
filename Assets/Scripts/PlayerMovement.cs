using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;
    [SerializeField] private float _mouseSensitivity;

    private Rigidbody _rigidbody;
    private PlayerInputActions _playerInputActions;
    private float _currentRotation;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Jump.performed += Jump;
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

        transform.position += directionVector * moveDistance;
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
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}
