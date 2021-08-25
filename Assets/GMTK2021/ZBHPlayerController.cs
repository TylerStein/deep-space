using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ZBHPlayerController : MonoBehaviour
{
    public Vector2 edgePadding = new Vector2(24f, 24f);

    public ZBHDirector zbhDirector;
    public Camera zbhCamera;
    public ZBHBackgroundController zbhBackground;
    public Transform zbhCore;
    public Transform zbhPoint;
    public Transform playerModel;

    public Vector2 screenMouse = Vector2.zero;
    public Vector3 worldMouse = Vector3.zero;

    public void Start() {
        if (!zbhCamera) zbhCamera = Camera.main;
    }

    // Update is called once per frame
    public void OnUpdatePointerPosition(InputAction.CallbackContext context) {
        if (!zbhDirector.isPlaying) return;

        screenMouse = context.ReadValue<Vector2>();
        Vector2 screen = new Vector2(Screen.width, Screen.height);

        if (screenMouse.x < edgePadding.x) screenMouse.x = edgePadding.x;
        else if (screenMouse.x > (screen.x - edgePadding.x)) screenMouse.x = screen.x - edgePadding.x;

        if (screenMouse.y < edgePadding.y) screenMouse.y = edgePadding.y;
        else if (screenMouse.y > (screen.y - edgePadding.y)) screenMouse.y = screen.y - edgePadding.y;

        zbhBackground.SetView(screenMouse.x / Screen.width, screenMouse.y / Screen.height);


        if (zbhCamera.orthographic) {
            worldMouse = zbhCamera.ScreenToWorldPoint(screenMouse);
            worldMouse.z = 0;
        } else {
            var ray = zbhCamera.ScreenPointToRay(screenMouse);
            worldMouse = zbhCamera.transform.position + (ray.direction * 1f);
            worldMouse.z = 0;
        }

        zbhPoint.position = worldMouse;
        playerModel.LookAt(zbhCore, Vector3.back);
    }
}
