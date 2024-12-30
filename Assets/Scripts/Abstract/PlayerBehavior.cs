using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBehavior : MonoBehaviour
{
    //Healthpoints
    protected int healthPoints = 20;

    //Movement behavior values
    protected float moveSpeed = 5f;
    protected float groundAcceleration = 10f;
    protected float airAcceleration = 2f;
    protected float jetPackPower = 3f;
    protected float jetPackMaxDuration = 3f;
    protected float jetPackDuration = 0f;
    protected Vector3 velocity;

    //TopDown ability values
    protected bool isTopDown = false;
    protected bool isTransitionOngoing = false;


    //Grounded values
    protected bool isGrounded;
    protected float groundCheckRange = 1f; //groundCheckRange needs to be re-set after Player models are done


    //Input
    protected GameInput gameInput;
    protected InputAction move;
    protected InputAction jetPack;
    protected InputAction topDownAbility;
    protected InputAction shoot;


    //Gameobjects
    protected Rigidbody rb;
    protected Transform cameraTransform;
    protected CameraBehavior cameraBehavior;
    protected GameObject weaponEnds;
    protected Transform[] barrels;
    protected int barrelsIndex = 1; //because 0 is always the parent object of the barrels. thanks unity...


    protected virtual void Awake()
    {

        //Gameinput
        gameInput = new GameInput();
        move = gameInput.Player.Move;
        jetPack = gameInput.Player.Jump;
        topDownAbility = gameInput.Player.TopDownAbility;
        shoot = gameInput.Player.Attack;

        //GetCompontents
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        cameraBehavior = Camera.main.GetComponent<CameraBehavior>();
        weaponEnds = GameObject.Find("WeaponEnds");
        barrels = weaponEnds.GetComponentsInChildren<Transform>();

    }

    protected void OnEnable()
    {
        //Enable Inputs
        move.Enable();
        jetPack.Enable();
        topDownAbility.Enable();
        shoot.Enable();

    }

    protected void OnDisable()
    {
        //Disable Inputs
        move.Disable();
        jetPack.Disable();
        topDownAbility.Disable();
        shoot.Disable();
    }

    protected virtual void Update()
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
            velocity = rb.velocity; //Needed so that velocity is still beeing updated even if movement is locked
        }

    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = velocity;

        //Debug stuff
        //Debug.Log(jetPackDuration);
        //Debug.Log(rb.velocity);

    }

    /// <summary>
    /// First-Person horizontal movement
    /// </summary>
    protected void FPHorizontalMovement()
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

    /// <summary>
    /// First-Person vertical movement
    /// </summary>
    protected void FPVerticalMovement()
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
