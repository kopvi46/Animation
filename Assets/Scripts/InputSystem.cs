using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;

    public event EventHandler OnJumpPerformed;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Jump.performed += Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnJumpPerformed?.Invoke(this, EventArgs.Empty);
        }
    }   
}
