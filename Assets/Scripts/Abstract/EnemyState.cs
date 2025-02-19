using System.Collections;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    //RB and velocity
    protected Rigidbody rb;
    protected Vector3 velocity;

    //Enemy behaviour scripts
    protected EnemyBehaviour enemyBehaviour;

    //Transfrom components
    protected Transform lowerBody;
    protected Transform upperBody;
    protected Transform barrel;
    protected Transform weaponEnd;

    //Wall detection values
    protected float wallCheckRange = 3f;
    protected bool isWallInFront;
    protected bool isWallBehind;

    #region Enemey Stats
    //Enemy movement
    protected float enemyMoveSpeed;
    protected float enemyGroundAcceleration;
    protected float enemyAirAcceleration;

    //Enemy damage
    protected float enemyProjectileDamage;
    protected float enemyMissleDamage;
    protected float enemyExplosionDamage;
    protected float enemyExplosionRadius;

    //Enemy firerate
    protected float enemyFirerate;
    protected float shootCooldown;

    private float burstShootCooldown = 0.2f;
    private int shotCount = 0;
    private int burstShots;

    //Player detection values
    protected float detectionRange;
    protected float startShootAngle; //At what angle the enemy already start shooting before it can completly lock onto the player

    //Enemy turning values
    protected float rotateSmoothness;
    protected float rotateWeaponSmothness;
    protected float rotateAngle = 20f;

    //Player detection values
    protected Vector3 playerDirection;
    protected Vector3 playerLastSeenPostion;
    protected bool playerDetected = false;
    protected bool playerOnceSeen = false;
    protected bool startShoot = false;
    protected int playerLayerMaskIndex;
    #endregion

    //Player
    protected GameObject player;

    //Projectile
    protected GameObject projectile;


    protected virtual void Awake()
    {
        //Get rb and scripts
        rb = GetComponent<Rigidbody>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();

        //Get player
        player = enemyBehaviour.Player;

        //Get projectile
        projectile = enemyBehaviour.Projectile;

        //Get index for player layer
        playerLayerMaskIndex = LayerMask.NameToLayer("Player");

        //Start a coroutine that only runs every seconds
        StartCoroutine(OneSecUpdate());

        //Gets all transforms
        lowerBody = enemyBehaviour.lowerBody;
        upperBody = enemyBehaviour.upperBody;
        barrel = enemyBehaviour.barrel;
        weaponEnd = enemyBehaviour.weaponEnd;

        //Gets all stats
        GetStats();
    }

    protected virtual void Update()
    {
        //Check if walls are in front or behind
        WallCheck();
    }

    protected virtual void FixedUpdate()
    {
        //Velocity apply
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    /// <summary>
    /// A coroutine that runs every second
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator OneSecUpdate()
    {
        while (true)
        {

            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Checks for walls in a cone in front and behind
    /// </summary>
    protected void WallCheck()
    {
        //The position from where the check rays start
        Vector3 wallDetectionPosition = new Vector3(lowerBody.position.x, lowerBody.position.y + 0.1f, lowerBody.position.z);

        //The 3 front rays
        Vector3 front = lowerBody.forward;
        Vector3 frontLeft = (lowerBody.forward + lowerBody.right * 0.5f);
        Vector3 frontRight = (lowerBody.forward - lowerBody.right * 0.5f);
        //The 3 back rays
        Vector3 back = -lowerBody.forward;
        Vector3 backLeft = (-lowerBody.forward - lowerBody.right * 0.5f);
        Vector3 backRight = (-lowerBody.forward + lowerBody.right * 0.5f);

        //All checks
        bool checkFront = Physics.Raycast(wallDetectionPosition, front, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkFrontLeft = Physics.Raycast(wallDetectionPosition, frontLeft, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkFrontRight = Physics.Raycast(wallDetectionPosition, frontRight, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkBack = Physics.Raycast(wallDetectionPosition, back, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkBackLeft = Physics.Raycast(wallDetectionPosition, backLeft, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkBackRight = Physics.Raycast(wallDetectionPosition, backRight, wallCheckRange, LayerMask.GetMask("Ground"));

        //Set bool if wall in front
        if (checkFront || checkFrontLeft || checkFrontRight)
        {
            isWallInFront = true;
        }
        else
        {
            isWallInFront = false;
        }

        //Set bool if wall behind
        if (checkBack || checkBackLeft || checkBackRight)
        {
            isWallBehind = true;
        }
        else
        {
            isWallBehind = false;
        }
    }

    /// <summary>
    /// Move into a direction
    /// </summary>
    /// <param name="direction"></param>
    protected void Move(Direction direction)
    {
        Vector3 moveDirection = lowerBody.forward;

        if (direction == Direction.forward)
        {
            velocity.x = Mathf.Lerp(rb.velocity.x, moveDirection.x * enemyMoveSpeed, enemyGroundAcceleration * Time.deltaTime);
            velocity.z = Mathf.Lerp(rb.velocity.z, moveDirection.z * enemyMoveSpeed, enemyGroundAcceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.Lerp(rb.velocity.x, -moveDirection.x * enemyMoveSpeed, enemyGroundAcceleration * Time.deltaTime);
            velocity.z = Mathf.Lerp(rb.velocity.z, -moveDirection.z * enemyMoveSpeed, enemyGroundAcceleration * Time.deltaTime);
        }
    }

    /// <summary>
    /// Rotate the bottom into a direction
    /// </summary>
    /// <param name="direction"></param>
    protected void RotateBottom(Direction direction)
    {
        float newAngle;
        Vector3 currentDirection = lowerBody.forward;
        if (direction == Direction.right)
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg) + rotateAngle; //new angle to the right
        }
        else
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg) - rotateAngle; //new angle to the left
        }
        Quaternion newDirection = Quaternion.AngleAxis(newAngle, Vector3.up); //new rotation
        lowerBody.rotation = Quaternion.RotateTowards(lowerBody.rotation, newDirection, rotateSmoothness * Time.deltaTime); //apply rotation
    }

    /// <summary>
    /// Rotates the bottom towards the player
    /// </summary>
    protected void RotateBottomTowardsPlayer()
    {
        Vector3 direction = (playerLastSeenPostion - lowerBody.position).normalized; //direction of the player
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; //angle diffrence towards the player
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up); //rotation towards the player
        lowerBody.rotation = Quaternion.RotateTowards(lowerBody.rotation, newDirection, rotateSmoothness * Time.deltaTime); //apply rotation
    }

    /// <summary>
    /// Rotates the top into a direction
    /// </summary>
    /// <param name="direction"></param>
    protected void RotateTop(Direction direction)
    {
        float newAngle;
        Vector3 currentDirection = upperBody.forward;
        if (direction == Direction.right)
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.y) * Mathf.Rad2Deg) + rotateAngle; //new angle to the right
        }
        else
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.y) * Mathf.Rad2Deg) - rotateAngle; //new angle to the left
        }
        Quaternion newDirection = Quaternion.AngleAxis(newAngle, Vector3.up); //new rotation
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime); //apply rotation
    }

    /// <summary>
    /// Rotates the top and the weapon towards the player
    /// </summary>
    protected void RotateTopAndWeaponTowardsPlayer()
    {
        //Rotation of upper body towards player
        Vector3 direction = (playerLastSeenPostion - upperBody.position).normalized; //direction of the player
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; //angle diffrence towards the player
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up); //rotation towards the player
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);//apply rotation

        //Rotation of weapon towards player
        Vector3 direction2 = (playerLastSeenPostion - barrel.position).normalized; //direction of the player
        Quaternion newDirection2 = Quaternion.LookRotation(direction2, Vector3.up); //rotation towards the player
        barrel.rotation = Quaternion.RotateTowards(barrel.rotation, newDirection2, rotateWeaponSmothness * Time.deltaTime);//apply rotation
        barrel.eulerAngles = new Vector3(barrel.eulerAngles.x, upperBody.eulerAngles.y, upperBody.eulerAngles.z);//apply rotation
    }

    /// <summary>
    /// Shoots one projectile every firrate cooldown into direction the barrel points
    /// </summary>
    protected void Shoot()
    {
        if (shootCooldown == 0)
        {
            GameObject bulletClone = Instantiate(projectile, weaponEnd.position, weaponEnd.rotation); //Spawn bullet
            bulletClone.GetComponent<ProjectileBehaviour>().Damage = enemyMissleDamage; //Give damage value to bullet
            shootCooldown = enemyFirerate; //Cooldown

        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }

    /// <summary>
    /// Shoots a burst of projectiles every firerate cooldown into the direction the barrel points
    /// </summary>
    protected void BurstShoot()
    {
        if (shootCooldown == 0)
        {
            GameObject bulletClone = Instantiate(projectile, weaponEnd.position, weaponEnd.rotation); //Spawn bullet
            bulletClone.GetComponent<ProjectileBehaviour>().Damage = enemyMissleDamage; //Give damage value to bullet
            shotCount++; //Burst shot count up
            if (shotCount < burstShots) //If burst not finished -> small cooldown
            {
                shootCooldown = burstShootCooldown;
            }
            else //If burst finished -> normal cooldown and reset the burst count
            {
                shootCooldown = enemyFirerate;
                shotCount = 0;
            }

        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }

    /// <summary>
    /// Gets all the stats for the enemy
    /// </summary>
    private void GetStats()
    {
        enemyMoveSpeed = enemyBehaviour.EnemyMoveSpeed;
        enemyGroundAcceleration = enemyBehaviour.EnemyGroundAcceleration;
        enemyAirAcceleration = enemyBehaviour.EnemyAirAcceleration;
        enemyProjectileDamage = enemyBehaviour.EnemyProjectileDamage;
        enemyMissleDamage = enemyBehaviour.EnemyMissleDamage;
        enemyExplosionDamage = enemyBehaviour.EnemyExplosionDamage;
        enemyExplosionRadius = enemyBehaviour.EnemyExplosionRadius;
        enemyFirerate = enemyBehaviour.EnemyFirerate;
        burstShots = enemyBehaviour.BurstShots;
        detectionRange = enemyBehaviour.DetectionRange;
        startShootAngle = enemyBehaviour.StartShootAngle;
        rotateSmoothness = enemyBehaviour.RotateSmoothness;
        rotateWeaponSmothness = enemyBehaviour.RotateWeaponSmothness;
    }
}
