using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public abstract class EnemyState : MonoBehaviour
{
    protected Rigidbody rb;
    protected Vector3 velocity;

    protected EnemyBehavior enemyBehavior;


    protected Transform lowerBody;
    protected Transform upperBody;
    protected Transform barrel;
    protected Transform weaponEnd;

    protected float wallCheckRange = 3f;
    public bool isWallInFront;
    public bool isWallBehind;

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
    private int burstShots = 3;

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

    protected GameObject player;
    protected GameObject projectile;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyBehavior = GetComponent<EnemyBehavior>();
        player = enemyBehavior.Player;
        projectile = enemyBehavior.Projectile;

        playerLayerMaskIndex = LayerMask.NameToLayer("Player");


        StartCoroutine(OneSecUpdate());

        player = enemyBehavior.Player;
        projectile = enemyBehavior.Projectile;
        lowerBody = enemyBehavior.lowerBody;
        upperBody = enemyBehavior.upperBody;
        barrel = enemyBehavior.barrel;
        weaponEnd = enemyBehavior.weaponEnd;
        GetStats();
    }

    protected virtual void Update()
    {
        WallCheck();
    }

    protected virtual void FixedUpdate()
    {
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    protected virtual IEnumerator OneSecUpdate()
    {
        while (true)
        {





            yield return new WaitForSeconds(1f);
        }
    }

    protected void WallCheck()
    {
        Vector3 wallDetectionPosition = new Vector3 (lowerBody.position.x, lowerBody.position.y + 0.1f, lowerBody.position.z);

        Vector3 front = lowerBody.forward;
        Vector3 frontLeft = (lowerBody.forward + lowerBody.right * 0.5f);
        Vector3 frontRight = (lowerBody.forward - lowerBody.right * 0.5f);
        Vector3 back = -lowerBody.forward;
        Vector3 backLeft = (-lowerBody.forward - lowerBody.right * 0.5f);
        Vector3 backRight = (-lowerBody.forward + lowerBody.right * 0.5f);

        bool checkFront = Physics.Raycast(wallDetectionPosition, front, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkFrontLeft = Physics.Raycast(wallDetectionPosition, frontLeft, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkFrontRight = Physics.Raycast(wallDetectionPosition, frontRight, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkBack = Physics.Raycast(wallDetectionPosition, back, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkBackLeft = Physics.Raycast(wallDetectionPosition, backLeft, wallCheckRange, LayerMask.GetMask("Ground"));
        bool checkBackRight = Physics.Raycast(wallDetectionPosition, backRight, wallCheckRange, LayerMask.GetMask("Ground"));

        if (checkFront || checkFrontLeft || checkFrontRight)
        {
            isWallInFront = true;
        }
        else
        {
            isWallInFront = false;
        }

        if (checkBack || checkBackLeft || checkBackRight)
        {
            isWallBehind = true;
        }
        else
        {
            isWallBehind = false;
        }
    }

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

    protected void RotateBottom(Direction direction)
    {
        float newAngle;
        Vector3 currentDirection = lowerBody.forward;
        if (direction == Direction.right)
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg) + rotateAngle;
        }
        else
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg) - rotateAngle;
        }
        Quaternion newDirection = Quaternion.AngleAxis(newAngle, Vector3.up);
        lowerBody.rotation = Quaternion.RotateTowards(lowerBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);
    }

    protected void RotateBottomTowardsPlayer()
    {
        Vector3 direction = (playerLastSeenPostion - lowerBody.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        lowerBody.rotation = Quaternion.RotateTowards(lowerBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);
    }

    protected void RotateTop(Direction direction)
    {
        float newAngle;
        Vector3 currentDirection = upperBody.forward;
        if(direction == Direction.right)
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.y) * Mathf.Rad2Deg) + rotateAngle;
        }
        else
        {
            newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.y) * Mathf.Rad2Deg) - rotateAngle;
        }
        Quaternion newDirection = Quaternion.AngleAxis(newAngle, Vector3.up);
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);
    }

    protected void RotateTopAndWeaponTowardsPlayer()
    {
        //Rotation of upper body towards player
        Vector3 direction = (playerLastSeenPostion - upperBody.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);

        //Rotation of weapon towards player
        Vector3 direction2 = (playerLastSeenPostion - barrel.position).normalized;
        Quaternion newDirection2 = Quaternion.LookRotation(direction2, Vector3.up);
        barrel.rotation = Quaternion.RotateTowards(barrel.rotation, newDirection2, rotateWeaponSmothness * Time.deltaTime);
        barrel.eulerAngles = new Vector3(barrel.eulerAngles.x, upperBody.eulerAngles.y, upperBody.eulerAngles.z);
    }

    protected void Shoot()
    {
        if (shootCooldown == 0)
        {
            Instantiate(projectile, weaponEnd.position, weaponEnd.rotation);
            shootCooldown = enemyFirerate;

        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }

    protected void BurstShoot()
    {
        if (shootCooldown == 0)
        {
            Instantiate(projectile, weaponEnd.position, weaponEnd.rotation);
            shotCount++;
            if (shotCount < burstShots)
            {
                shootCooldown = burstShootCooldown;
            }
            else
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

    private void GetStats()
    {
        enemyMoveSpeed = enemyBehavior.EnemyMoveSpeed;
        enemyGroundAcceleration = enemyBehavior.EnemyGroundAcceleration;
        enemyAirAcceleration = enemyBehavior.EnemyAirAcceleration;
        enemyProjectileDamage = enemyBehavior.EnemyProjectileDamage;
        enemyMissleDamage = enemyBehavior.EnemyMissleDamage;
        enemyExplosionDamage = enemyBehavior.EnemyExplosionDamage;
        enemyExplosionRadius = enemyBehavior.EnemyExplosionRadius;
        enemyFirerate = enemyBehavior.EnemyFirerate;
        burstShots = enemyBehavior.BurstShots;
        detectionRange = enemyBehavior.DetectionRange;
        startShootAngle = enemyBehavior.StartShootAngle;
        rotateSmoothness = enemyBehavior.RotateSmoothness;
        rotateWeaponSmothness = enemyBehavior.RotateWeaponSmothness;
    }


}
