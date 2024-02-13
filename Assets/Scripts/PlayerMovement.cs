using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Outside Sources")]
    PlayerActionsScript playerActions;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject playerBody;

    [Header("Cutscene")]
    [SerializeField] bool inCutscene;

    [Header("Movement")]
    [SerializeField] float speed;
    Vector3 playerVelocity;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float rotationSpeed = 5f;

    private void Awake()
    {
        playerActions = new PlayerActionsScript();
        OnEnable();
    }

    #region Cutscene Get/Set
    public void SetCutscene(bool cutscene)
    {
        inCutscene = cutscene;

        if(inCutscene)
        {
            OnDisable();
            return;
        }

        OnEnable();
    }

    public bool GetCutscene()
    {
       return inCutscene;
    }
    #endregion

    #region On Enable/Disable
    public void OnEnable()
    {
        playerActions.Player.Enable();
        playerActions.Player.Jump.performed += Jump;
    }

    public void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Jump.performed -= Jump;
    }
    #endregion

    private void Update()
    {
        ProcessMovement();

    }

    private void ProcessMovement()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2;

        Vector2 inputVector = playerActions.Player.Movement.ReadValue<Vector2>();
        Vector3 movementDir = (new Vector3(inputVector.x,0,inputVector.y)).normalized;

        if(inputVector.magnitude != 0)
        {
            float angle = Vector3.SignedAngle(playerBody.transform.forward, movementDir, Vector3.up);
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
            playerBody.transform.rotation = Quaternion.Lerp(playerBody.transform.rotation, playerBody.transform.rotation * rot, Time.deltaTime * rotationSpeed);
        }

        playerVelocity.y += gravity * Time.deltaTime;

        Vector3 moveInputVal = transform.TransformDirection(movementDir) * speed;
        playerVelocity.x = moveInputVal.x;
        playerVelocity.z = moveInputVal.z;

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(characterController.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

}
