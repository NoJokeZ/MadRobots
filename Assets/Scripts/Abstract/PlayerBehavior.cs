using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public abstract class PlayerBehavior : MonoBehaviour
{

    public PlayerType myType { get; protected set; }

    //Damage values
    private bool isInvincible = false;
    private float invincibleTime = 0.50f;
    private float invincibleCounter = 0f;

    //PlayerStats
    //Healthpoints
    public float HealthPoints { get; protected set; }
    protected float healthRegeneration;

    //Armor
    protected int bulletArmorPoints;
    protected int explosionArmorPoints;

    //Movement
    protected float moveSpeed;
    protected float groundAcceleration;
    protected float airAcceleration;
    protected float jetPackPower;
    public float JetPackMaxDuration { get; protected set; }

    public float JetPackDuration { get; protected set; }

    //Cooldown values
    protected float shootCooldown;
    protected float firrate;

    //Ammo values
    protected int specialAbilityAmmo;

    //TopDown ability values
    public bool isTopDown = false;
    public bool isTransitionOngoing = false;


    //Grounded values
    protected bool isGrounded;
    protected float groundCheckRange = 1.2f; //1.1 is heigth of RocketV1 transform on ground

    //Velocity
    protected Vector3 velocity;

    //Input
    protected GameInput gameInput;
    protected InputAction move;
    protected InputAction jetPack;
    protected InputAction topDownAbility;
    protected InputAction shoot;


    //Gameobjects
    protected Rigidbody rb;
    protected Transform upperBody;
    protected Transform weapons;
    protected Transform cameraTransform;
    protected CameraBehavior cameraBehavior;
    protected GameObject weaponEnds;
    protected Transform[] barrels;
    protected int barrelsIndex = 1; //because 0 is always the parent object of the barrels. thanks unity...
    private GameManager gameManager;
    private GameObject jetPackVisuals;


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
        upperBody = transform.Find("UpperBody");
        weapons = transform.Find("UpperBody/Weapons");
        cameraTransform = Camera.main.transform;
        cameraBehavior = Camera.main.GetComponent<CameraBehavior>();
        weaponEnds = transform.Find("UpperBody/Weapons/WeaponEnds").gameObject;
        barrels = weaponEnds.GetComponentsInChildren<Transform>();
        gameManager = GameManager.Instance;
        jetPackVisuals = transform.Find("LowerBody/JetPackVisuals").gameObject;
        jetPackVisuals.SetActive(false);
    }

    protected void OnEnable()
    {
        //Enable Inputs
        EnableControls();

        GetPlayerStats();
    }

    protected void OnDisable()
    {
        //Disable Inputs
        DisableControls();
    }

    protected virtual void Update()
    {
        isTopDown = cameraBehavior.IsTopDown;
        isTransitionOngoing = cameraBehavior.IsTransitionOngoing;


        //Rotate player model
        transform.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0);

        //Movement
        if (!isTopDown && !isTransitionOngoing)
        {
            FPHorizontalMovement();
            FPVerticalMovement();
            weapons.transform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, upperBody.eulerAngles.y, 0);
        }
        else
        {
            velocity = rb.velocity; //Needed so that velocity is still beeing updated even if movement is locked
        }

        if (isInvincible)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0) isInvincible = false;
        }

        DestroyOnWorldExit();
        CheckHP();
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
        velocity.x = Mathf.Lerp(rb.velocity.x, moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
        velocity.z = Mathf.Lerp(rb.velocity.z, moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
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
            JetPackDuration += Time.deltaTime;
            if (JetPackDuration > JetPackMaxDuration) JetPackDuration = JetPackMaxDuration;
        }

        //Jetpack use logic
        if (jetPack.IsPressed() && JetPackDuration > 0)
        {
            velocity.y = jetPackPower;
            JetPackDuration -= Time.deltaTime;
            if (JetPackDuration < 0) JetPackDuration = 0; //no values underneat 0 for fuel display
            jetPackVisuals.SetActive(true);
        }
        else
        {
            jetPackVisuals.SetActive(false);
        }
        
    }

    /// <summary>
    /// Check for projectile hits
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile") && !isInvincible)
        {
            isInvincible = true;
            invincibleCounter = invincibleTime;
            float damage = collision.gameObject.GetComponent<ProjectileBehavior>().Damage;
            TakeDamage(damage, DamageType.Bullet);
        }
    }

    /// <summary>
    /// Check for explosion hits
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Explosion") && !isInvincible)
        {
            isInvincible = true;
            invincibleCounter = invincibleTime;

            float damage = other.gameObject.GetComponent<ExplosionBehavior>().Damage;
            TakeDamage(damage, DamageType.Explosion);
        }
    }

    /// <summary>
    /// Default take damage
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void TakeDamage(float damage, DamageType damageType)
    {
        if (damageType.Equals(DamageType.Bullet))
        {
            damage -= bulletArmorPoints;
            if (damage <= 0)
            {
                //Bullet blocked -> UI
            }
            else
            {
                //Bullet hit -> UI
                HealthPoints -= damage;
            }
        }
        else if (damageType.Equals(DamageType.Explosion))
        {
            damage -= explosionArmorPoints;
            if (damage <= 0)
            {
                //Explosion blocked -> UI
            }
            else
            {
                //Explosion hit -> UI
                HealthPoints -= damage;
            }
        }
    }

    /// <summary>
    /// Checks HP and destroys itself if 0 or less
    /// </summary>
    private void CheckHP()
    {
        if (HealthPoints <= 0)
        {
            gameManager.GameEnd();
            Destroy(gameObject);
        }
    }

    private void DestroyOnWorldExit()
    {
        if (transform.position.y <= -20)
        {
            gameManager.GameEnd();
            Destroy(gameObject);
        }
    }

    protected virtual void GetPlayerStats()
    {
        //Get Values
        //Healthpoints
        HealthPoints = UpgradeManager.Instance.PlayerMaxHealth;
        healthRegeneration = UpgradeManager.Instance.PlayerHealthRegeneration;

        //Armor
        bulletArmorPoints = UpgradeManager.Instance.PlayerBulletArmorPoints;
        explosionArmorPoints = UpgradeManager.Instance.PlayerExplosionArmorPoints;

        //Movement behavior values
        moveSpeed = UpgradeManager.Instance.PlayerMoveSpeed;
        groundAcceleration = UpgradeManager.Instance.PlayerGroundAcceleration;
        airAcceleration = UpgradeManager.Instance.PlayerAirAcceleration;
        jetPackPower = UpgradeManager.Instance.PlayerJetPackPower;
        JetPackMaxDuration = UpgradeManager.Instance.PlayerJetPackMaxDuration;

        //Firerate
        firrate = UpgradeManager.Instance.PlayerFirerate;

        //Ammo
        specialAbilityAmmo = UpgradeManager.Instance.PlayerSpecialAbilityAmmo;

    }


    public void EnableControls()
    {
        move.Enable();
        jetPack.Enable();
        topDownAbility.Enable();
        shoot.Enable();
    }

    public void DisableControls()
    {
        move.Disable();
        jetPack.Disable();
        topDownAbility.Disable();
        shoot.Disable();
    }
}
