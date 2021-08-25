using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonInput
{
    public bool downThisFrame;
    public int framesDown;
    public bool down;

    public void Cancel() {
        down = false;
        downThisFrame = false;
        framesDown = 0;
    }

    public void Update() {
        if (down || downThisFrame == true) {
            if (framesDown > 0) {
                downThisFrame = false;
            }
            framesDown++;
        }
    }

    public void UpdateFromPhase(InputActionPhase phase) {
        switch (phase) {
            case InputActionPhase.Started:
                down = true;
                framesDown = 0;
                downThisFrame = true;
                break;
            case InputActionPhase.Canceled:
                down = false;
                break;
            default: return;
        }
    }
}

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PrioritizedList<GameObject> listeners = new PrioritizedList<GameObject>();
    public Vector2 move;
    public Vector2 look;
    public ButtonInput fire = new ButtonInput();
    public ButtonInput pause = new ButtonInput();

    public void Update() {
        pause.Update();
        fire.Update();
    }

    public bool IsActiveListener(GameObject obj) {
        return listeners.Top() == obj;
    }

    public void AddInputListener(GameObject listener, int priority) {
        listeners.Add(listener, priority);
    }

    public void RemoveActiveListener(GameObject listener) {
        listeners.Remove(listener);
    }

    #region Device
    public void OnDeviceLost(PlayerInput context) {
        Debug.Log("Device Lost");
    }

    public void OnDeviceRegained(PlayerInput context) {
        Debug.Log("Device Regained");
    }
    #endregion

    #region Inputs
    public void OnMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) {
        look = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context) {
        fire.UpdateFromPhase(context.phase);
    }

    public void OnPause(InputAction.CallbackContext context) {
        pause.UpdateFromPhase(context.phase);
    }
    #endregion
}
