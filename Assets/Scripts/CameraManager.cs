using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    PlayerActionsScript playerActions;

    [Header("Following Player")]
    [SerializeField] Transform target;
    float distanceToPlayer;

    [Header("Orbiting Player")]
    [SerializeField] MouseSensitivity mouseSensitivity;
    [SerializeField] CameraAngle cameraAngle;
    Vector2 input;
    CameraRotation cameraRot;

    private void Awake()
    {
        playerActions = new PlayerActionsScript();
        distanceToPlayer = Vector3.Distance(transform.position, target.position);
        OnEnable();
    }

    public void OnEnable()
    {
        playerActions.Player.Enable();
        playerActions.Player.Look.performed += Look;
    }

    public void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Look.performed -= Look;
    }

    public void Look(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        cameraRot.yaw += input.x * mouseSensitivity.horizontal * Time.deltaTime;
        cameraRot.pitch += input.y * -mouseSensitivity.vertical * Time.deltaTime;
        cameraRot.pitch = Mathf.Clamp(cameraRot.pitch, cameraAngle.min, cameraAngle.max);
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(cameraRot.pitch, cameraRot.yaw, 0.0f);
        transform.position = target.position - transform.forward * distanceToPlayer;
    }


}

[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
}

public struct CameraRotation
{
    public float pitch;
    public float yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min;
    public float max;
}