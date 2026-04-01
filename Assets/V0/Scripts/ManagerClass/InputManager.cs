using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public event Action<Vector3> OnMove;
    public event Action<Vector2> OnLook;
    public event Action OnJump;
    public event Action OnInteracr;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            OnMove?.Invoke(context.ReadValue<Vector3>());
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            OnLook?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJump?.Invoke();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteracr?.Invoke();
        }
    }
}
