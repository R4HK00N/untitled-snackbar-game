using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class CameraMovement : NetworkBehaviour
{
    public Transform player;
    public float cameraSens = 2f;
    float cameraVerticalRotation = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (!IsOwner) return;
        CameraMove();
    }
    void CameraMove()
    {
        float inputX = Input.GetAxis("Mouse X") * cameraSens;
        float InputY = Input.GetAxis("Mouse Y") * cameraSens;

        cameraVerticalRotation -= InputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        player.Rotate(Vector3.up * inputX);
    }
}
