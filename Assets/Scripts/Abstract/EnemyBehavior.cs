using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    public float HealtPoints { get; protected set; } = 10;

    //Explosion damage values
    private bool isExplosionDamageDelay = false;
    private float explosionDamageDelayTime = 0.25f;
    private float explosionDamageDeleyCounter = 0f;

    //Shoot values
    protected float shootInterval = 2f;
    protected float shootCooldown;

    //Player detection values
    protected Vector3 playerDirection;
    protected Vector3 playerLastSeenPostion;
    protected float detectionRange = 10f;
    protected float startShootAngle = 20f; //At what angle the enemy already start shooting before it can completly lock onto the player
    protected bool playerDetected = false;
    protected bool playerOnceSeen = false;
    protected bool startShoot = false;
    protected int playerLayerMaskIndex;

    //Rotation behavior
    protected float rotateSmoothness = 20f;
    protected float rotateWeaponSmothness = 30f;

    //Movement values
    protected float moveSpeed = 3f;
    protected float acceleration = 5f;


    //Gameobjects and components
    protected GameObject player { get; private set; }
    protected GameManager gameManager;
    protected bool isPlayerAlive;
    protected GameObject projectile;
    protected Rigidbody rb;


    protected virtual void Awake()
    {
        GetPlayerObjects();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        playerLayerMaskIndex = LayerMask.NameToLayer("Player");
    }

    protected virtual void Update()
    {

        if (player == null)
        {
            GetPlayerObjects();
        }
        //HP and damage checks
        CheckHP();
        if (isExplosionDamageDelay)
        {
            ExplosionDamageDelayCountdown();
        }

        //Player dection and shoot state
        CheckCanSeePlayer();
        if (playerOnceSeen)
        {
            RotateTowardsPlayer();
        }
        if (playerDetected)
        {
            CheckShouldShoot();
        }
        if (startShoot)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Default take damage
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void TakeDamage(float damage)
    {
        HealtPoints -= damage;
    }

    /// <summary>
    /// Checks HP and destroys itself if 0 or less
    /// </summary>
    private void CheckHP()
    {
        if (HealtPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Check for projectile hits
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            float damage = collision.gameObject.GetComponent<ProjectileBehavior>().Damage;
            TakeDamage(damage);
        }
    }

    /// <summary>
    /// Check for explosion hits
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Explosion") && !isExplosionDamageDelay)
        {
            isExplosionDamageDelay = true; //Needed so that explosions can't deal multiple damage if multiple bodyparts are hit
            explosionDamageDeleyCounter = explosionDamageDelayTime;

            float damage = other.gameObject.GetComponent<ExplosionBehavior>().Damage;
            TakeDamage(damage);
        }
    }

    /// <summary>
    /// Calculation duration of explosion damage delay
    /// </summary>
    private void ExplosionDamageDelayCountdown()
    {
        explosionDamageDeleyCounter -= Time.deltaTime;
        if (explosionDamageDeleyCounter <= 0)
        {
            isExplosionDamageDelay = false;
        }
    }

    /// <summary>
    /// Default rotate / Rotes complete body towards the player on y axis
    /// </summary>
    protected virtual void RotateTowardsPlayer()
    {

        Vector3 direction = (playerLastSeenPostion - transform.position).normalized;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newDirection, rotateSmoothness * Time.deltaTime);

    }

    /// <summary>
    /// Default check if can see player
    /// </summary>
    protected virtual void CheckCanSeePlayer()
    {

        if (player != null)
        {
            playerDirection = (player.transform.position - transform.position).normalized;
        }
        //Debug.DrawRay(transform.position, playerDirection * detectionRange, Color.red); //Debug

        if (Physics.Raycast(transform.position, playerDirection, out RaycastHit hitInfo, detectionRange, ~LayerMask.GetMask("Enemy")))
        {
            if (hitInfo.transform.gameObject.layer == playerLayerMaskIndex)
            {
                playerDetected = true;
                playerOnceSeen = true;
                playerLastSeenPostion = hitInfo.transform.position;
            }
        }
        else
        {
            playerDetected = false;
            startShoot = false;
        }
    }

    /// <summary>
    /// Default check if should shoot
    /// </summary>
    protected virtual void CheckShouldShoot()
    {
        Vector3 lookDirection = transform.forward;
        float angle = Vector3.Angle(lookDirection, playerDirection);

        if (angle <= startShootAngle)
        {
            startShoot = true;
        }
        else
        {
            startShoot = false;
        }
    }

    /// <summary>
    /// Default shoot
    /// </summary>
    protected virtual void Shoot()
    {
        if (shootCooldown == 0)
        {
            Instantiate(projectile, transform.position + Vector3.forward, transform.rotation);
            shootCooldown = shootInterval;

        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }

    private void GetPlayerObjects()
    {
        player = GameObject.FindWithTag("Player");
    }
}
