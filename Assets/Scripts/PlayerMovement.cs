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

    [Header("Camera")]
    Camera mainCAM;

    [Header("Cutscene")]
    [SerializeField] bool inCutscene;

    [Header("Movement")]
    [SerializeField] float speed;
    Vector3 playerVelocity;
    private Rigidbody rigidbodyLizard;
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float movingSpeed;
    [Header("Jump")]
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float gravity = -9.8f;

    [Header("Dashing")]
    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCoolDown;
    float dashCoolDownTimer;

    //Ricky's Code
    private Animator animator;
    public bool walking;

    [Header("Test - Delete after complete")]
    [SerializeField] bool test;
    float testFloat; 

    //Start animation test
    //End animation test

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodyLizard = GetComponent<Rigidbody>();
    }



    private void Awake()
    {
        playerActions = new PlayerActionsScript();
        OnEnable();
        mainCAM = Camera.main;
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
        playerActions.Player.Dash.performed += Dash;
    }

    public void OnDisable()
    {
        playerActions.Player.Disable();
        playerActions.Player.Jump.performed -= Jump;
        playerActions.Player.Dash.performed -= Dash;
    }
    #endregion

    private void Update()
    {
        ProcessMovement();
        /*
                if(test == true)
                {
                    movingSpeed = 5f;
                }
                else
                {
                    movingSpeed = 0f;
                }
        */

        if (Input.GetKeyDown(KeyCode.W))
        {           
            walking = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            walking = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {

            walking = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {

            walking = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            walking = true;

        }
        if (Input.GetKeyUp(KeyCode.S))
        {

            walking = false;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {

            walking = true;

        }
        if (Input.GetKeyUp(KeyCode.D))
        {

            walking = false;

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            animator.SetTrigger("Jump");

        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            animator.SetTrigger("Attack");

        }
        if (walking == true)
        {
            animator.SetBool("walking", true);
        }
        if (walking == false)
        {
            animator.SetBool("walking", false);
        }
    }

    private void ProcessMovement()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2;

        Vector2 inputVector = playerActions.Player.Movement.ReadValue<Vector2>();
        Vector3 movementDir = (new Vector3(inputVector.x,0,inputVector.y)).normalized;

        if(inputVector.magnitude != 0)
        {
            movementDir = Quaternion.Euler(0.0f, mainCAM.transform.eulerAngles.y, 0.0f) * new Vector3(inputVector.x, 0.0f, inputVector.y);
            var targetRotation = Quaternion.LookRotation(movementDir, Vector3.up);
            playerBody.transform.rotation = Quaternion.RotateTowards(playerBody.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

        playerVelocity.y += gravity * Time.deltaTime;

        Vector3 moveInputVal = transform.TransformDirection(movementDir) * speed;
        playerVelocity.x = moveInputVal.x;
        playerVelocity.z = moveInputVal.z;

        testFloat = playerVelocity.y + playerVelocity.z;

        characterController.Move(playerVelocity * Time.deltaTime);

        //animator.SetFloat("Speed", movingSpeed);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(characterController.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            animator.SetTrigger("Jump");
        }
        animator.ResetTrigger("Jump");
    }

    public void Dash(InputAction.CallbackContext context)
    {

    }

}
