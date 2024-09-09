using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private PlayerInputActions _playerInputActions;

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
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        Vector3 directionVector = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = _speed * Time.deltaTime;

        transform.position += directionVector * moveDistance;
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}
