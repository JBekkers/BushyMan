using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    private float camX; 
    private float camY;
    private Vector2 CamyMinMax = new Vector2(-10, 90);

    private float camDistance;
    private float camOffset = 6;
    
    public bool CursorLock;
    public float mouseSensitivity = 2;
    public Transform Player;
    public float Distance = 6;
    private Vector2 CamDistMax = new Vector2(3, 9);

    private void Start()
    {
        //invis and locked in place cursor
        if (CursorLock) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        camX += Input.GetAxis("Mouse X") * mouseSensitivity;
        camY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        camY = Mathf.Clamp(camY, CamyMinMax.x, CamyMinMax.y);

        Vector3 playerRotation = new Vector3(camY, camX);
        transform.eulerAngles = playerRotation;

        camDistance = Input.GetAxis("MouseScroll");
        camOffset = camOffset + -camDistance * 3f;
        camOffset = Mathf.Clamp(camOffset, CamDistMax.x, CamDistMax.y);

        transform.position = Player.position - transform.forward * (camOffset);
    }
}
