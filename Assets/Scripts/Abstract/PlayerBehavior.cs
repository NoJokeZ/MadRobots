using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{

    //Movement behavior values
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float groundAcceleration = 10f;
    [SerializeField] float airAcceleration = 2f;
    [SerializeField] float jetPackPower = 3f;
    [SerializeField] float jetPackMaxDuration = 3f;
    private float jetPackDuration = 0f;
    private Vector3 velocity;

    //TopDown ability values
    private bool isTopDown = false;
    private bool isTransitionOngoing = false;


    //Grounded values
    private bool isGrounded;
    private float groundCheckRange = 1f; //groundCheckRange needs to be re-set after Player models are done


    //Input
    private GameInput gameInput;
    private InputAction move;
    private InputAction jetPack;
    private InputAction topDownAbility; //WIP


    //Gameobjects
    private Rigidbody rb;
    private Transform cameraTransform;
    private CameraBehavior cameraBehavior;



    private void Awake()
    {

        //Gameinput
        gameInput = new GameInput();
        move = gameInput.Player.Move;
        jetPack = gameInput.Player.Jump;
        topDownAbility = gameInput.Player.TopDownAbility;

        //GetCompontents
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        cameraBehavior = Camera.main.GetComponent<CameraBehavior>();

    }

    private void OnEnable()
    {
        //Enable Inputs
        move.Enable();
        jetPack.Enable();
        topDownAbility.Enable();

    }

    private void OnDisable()
    {
        //Disable Inputs
        move.Disable();
        jetPack.Disable();
        topDownAbility.Disable();
    }

    private void Update()
    {
        isTopDown = cameraBehavior.IsTopDown;
        isTransitionOngoing = cameraBehavior.IsTransitionOngoing;


        //Rotate player model
        transform.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0); //If Player should be able to tilt completly 0 need to be set to current rotation. (more complicated of course...)


        //Movement
        if (!isTopDown && !isTransitionOngoing)
        {
            FPHorizontalMovement();
            FPVerticalMovement();
        }
        else
        {
            velocity = rb.velocity;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;

        //Debug stuff
        //Debug.Log(jetPackDuration);
        //Debug.Log(rb.velocity);

    }

    private void FPHorizontalMovement()
    {

        Vector3 moveDirection = Vector3.zero;

        //Get input
        moveDirection.z = move.ReadValue<Vector2>().y;
        moveDirection.x = move.ReadValue<Vector2>().x;

        //Look direction * input
        moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * moveDirection;
        
        //Velocity decision
        float acceleration = isGrounded ? groundAcceleration : airAcceleration;

        //smooth out velocity
        velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
        
    }


    private void FPVerticalMovement()
    {
        //Get current velocity.y
        velocity.y = rb.velocity.y;

        //Check if grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckRange, ~LayerMask.GetMask("Player")); //groundCheckRange needs to be re-set after Player models are done
        Debug.DrawRay(transform.position, Vector3.down * groundCheckRange, Color.red);

        //refuel Jetpack while grounded
        if (isGrounded)
        {
            jetPackDuration += Time.deltaTime;
            if (jetPackDuration > jetPackMaxDuration) jetPackDuration = jetPackMaxDuration;
        }

        //Jetpack use logic
        if (jetPack.IsPressed() && jetPackDuration > 0)
        {
            velocity.y = jetPackPower;
            jetPackDuration -= Time.deltaTime;
            if (jetPackDuration < 0) jetPackDuration = 0; //no values underneat 0 for fuel display
        }
    }

}
