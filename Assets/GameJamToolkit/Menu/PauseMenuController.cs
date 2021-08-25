using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public PlayerInputHandler input;
    public CanvasGroup canvasGroup;
    public bool openOnStart = false;

    private void Start() {
        if (openOnStart) {
            OpenMenu();
        } else {
            CloseMenu();
        }
    }

    private void Update() {
        if (input.IsActiveListener(gameObject)) {
            if (input.pause.downThisFrame) {
                input.pause.Cancel();
                CloseMenu();
            }
        }
    }

    public void OpenMenu() {
        Cursor.visible = true;
        input.AddInputListener(gameObject, 10);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void CloseMenu() {
        Cursor.visible = false;
        input.RemoveActiveListener(gameObject);
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
