using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    public EnemyType EnemyType { get; protected set; }

    //Explosion damage values
    private bool isExplosionDamageDelay = false;
    private float explosionDamageDelayTime = 0.25f;
    private float explosionDamageDeleyCounter = 0f;

    #region Enemy Stats
    //Enemy stats
    public float EnemyMaxHealth { get; protected set; }
    public float EnemyHealth { get; protected set; }
    public int EnemyHealthRegeneration { get; protected set; }

    //Enemy armor
    public int EnemyBulletArmorPoints { get; protected set; }
    public int EnemyExplosionArmorPoints { get; protected set; }

    //Enemy movement
    public float EnemyMoveSpeed { get; protected set; }
    public float EnemyGroundAcceleration { get; protected set; }
    public float EnemyAirAcceleration { get; protected set; }

    //Enemy damage
    public float EnemyProjectileDamage { get; protected set; }
    public float EnemyMissleDamage { get; protected set; }
    public float EnemyExplosionDamage { get; protected set; }
    public float EnemyExplosionRadius { get; protected set; }

    //Enemy firerate
    public float EnemyFirerate { get; protected set; }
    public int BurstShots { get; protected set; }

    //Player detection values
    public float DetectionRange { get; protected set; }
    public float StartShootAngle { get; protected set; } //At what angle the enemy already start shooting before it can completly lock onto the player

    //Enemy turning values
    public float RotateSmoothness { get; protected set; }
    public float RotateWeaponSmothness { get; protected set; }
    #endregion


    //EnemyStats
    protected EnemyStats enemyStats;



    //Gameobjects and components
    public GameObject Player { get; private set; }
    protected GameManager gameManager;
    protected EnemyManager enemyManager;
    protected bool isPlayerAlive;
    public GameObject Projectile { get; protected set; }
    protected Rigidbody rb;

    //Transform of enemy parts
    public Transform lowerBody { get; private set; }
    public Transform upperBody { get; private set; }
    public Transform barrel { get; private set; }
    public Transform weaponEnd { get; private set; }


    private float killCheckDelay = 2f;
    bool isKillCheckDelay = true;


    protected virtual void Awake()
    {
        GetPlayerObjects();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        rb = GetComponent<Rigidbody>();

        lowerBody = transform.Find("LowerBody");
        upperBody = transform.Find("LowerBody/UpperBody");
        barrel = transform.Find("LowerBody/UpperBody/Barrel");
        weaponEnd = transform.Find("LowerBody/UpperBody/Barrel/WeaponEnd");
    }

    protected virtual void Update()
    {

        if (Player == null)
        {
            GetPlayerObjects();
        }
        //HP and damage checks

        if (!isKillCheckDelay)
        {
            CheckHP();
            DestroyOnWorldExit();
        }
        else
        {
            DelayKillChecks();
        }

        if (isExplosionDamageDelay)
        {
            ExplosionDamageDelayCountdown();
        }
    }

    /// <summary>
    /// Default take damage
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void TakeDamage(float damage)
    {
        EnemyHealth -= damage;
    }

    /// <summary>
    /// Checks HP and destroys itself if 0 or less
    /// </summary>
    private void CheckHP()
    {
        if (EnemyHealth <= 0)
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


    private void GetPlayerObjects()
    {
        Player = GameObject.FindWithTag("Player");
    }

    private void DestroyOnWorldExit()
    {
        if (transform.position.y <= -20)
        {
            Destroy(gameObject);
        }
    }

    private void DelayKillChecks()
    {
        killCheckDelay -= Time.deltaTime;

        if (killCheckDelay <= 0) isKillCheckDelay = false;
    }

    private void OnDestroy()
    {
        enemyManager.EnemyDied();
    }

    protected void GetStats()
    {
        EnemyHealth = enemyStats.EnemyHealth;
        EnemyHealthRegeneration = enemyStats.EnemyHealthRegeneration;
        EnemyBulletArmorPoints = enemyStats.EnemyBulletArmorPoints;
        EnemyExplosionArmorPoints = enemyStats.EnemyExplosionArmorPoints;
        EnemyMoveSpeed = enemyStats.EnemyMoveSpeed;
        EnemyGroundAcceleration = enemyStats.EnemyGroundAcceleration;
        EnemyAirAcceleration = enemyStats.EnemyAirAcceleration;
        EnemyProjectileDamage = enemyStats.EnemyProjectileDamage;
        EnemyMissleDamage = enemyStats.EnemyMissleDamage;
        EnemyExplosionDamage = enemyStats.EnemyExplosionDamage;
        EnemyExplosionRadius = enemyStats.EnemyExplosionRadius;
        EnemyFirerate = enemyStats.EnemyFirerate;
        BurstShots = enemyStats.BurstShots;
        DetectionRange = enemyStats.DetectionRange;
        StartShootAngle = enemyStats.StartShootAngle;
        RotateSmoothness = enemyStats.RotateSmoothness;
        RotateWeaponSmothness = enemyStats.RotateWeaponSmothness;
    }
}
