using System;
using UnityEngine;

public class EnemyTankV1 : EnemyBehavior
{
    private Transform upperBody;
    private Transform weaponEnd;
    private Transform lowerBody;

    private Vector3 velocity;
    private int leftRight;
    private float leftRightAngle;

    private float burstShootCooldown = 0.2f;
    private int shotCount = 0;
    private int burstShots = 3;

    public int currentIdleState;
    private float idleStateCooldown;
    private float minIdleStateCooldown = 2f;
    private float maxIdleStateCooldown = 5f;
    public int currentAttackState;
    private float attackStateCooldown;
    private float minAttackStateCooldown = 1f;
    private float maxAttackStateCooldown = 3f;


    private enum IdleState
    {
        idle,
        move,
        turn
    }

    private enum AttackState
    {
        stand,
        follow,
        strave,
        backup
    }

    protected override void Awake()
    {
        base.Awake();
        HealtPoints = 15;
        upperBody = transform.Find("UpperBody");
        weaponEnd = transform.Find("UpperBody/Barrel/WeaponEnd");
        projectile = Resources.Load<GameObject>("EnemyeProjectile1");
        lowerBody = transform.Find("LowerBody");
        shootInterval = 4f;
        startShootAngle = 30f;
    }

    protected override void Update()
    {
        base.Update();

        if (!playerDetected)
        {
            ChooseIdleState();
        }
        else
        {
            ChooseAttackState();
        }


        if (!playerDetected)
        {
            switch (currentIdleState)
            {
                case 0: //idle
                    velocity = rb.velocity;
                    break;

                case 1: //move
                    Move();
                    break;

                case 2: //turn
                    velocity = rb.velocity;
                    Turn(leftRightAngle);
                    break;
            }
        }
        else if (playerDetected)
        {
            RotateLowerTowardsPlayer();

            switch (currentAttackState)
            {
                case 0: //still
                    velocity = rb.velocity;
                    break;
                case 1: //follow
                    Follow();
                    break;

                case 2: //strave
                    Strave();
                    break;

                case 3: //backup
                    BackUp();
                    break;
            }
        }
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    private void Move()
    {
        Vector3 moveDirection = lowerBody.forward;

        velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
    }

    private void Turn(float angle)
    {
        Vector3 currentDirection = lowerBody.forward;
        float newAngle = (Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg) + angle;
        Quaternion newDirection = Quaternion.AngleAxis(newAngle, Vector3.up);
        lowerBody.rotation = Quaternion.RotateTowards(lowerBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);
    }

    private void Follow()
    {
        Vector3 moveDirection = lowerBody.forward;

        velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
    }

    private void Strave()
    {

        Vector3 moveDirection = lowerBody.right;

        if (leftRight == 0) //Strave right
        {
            velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
        }
        else //Strave left
        {
            velocity.x = Mathf.Lerp(velocity.x, -moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, -moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
        }
    }

    private void BackUp()
    {
        Vector3 moveDirection = lowerBody.forward;

        velocity.x = Mathf.Lerp(velocity.x, -moveDirection.x * moveSpeed, acceleration * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, -moveDirection.z * moveSpeed, acceleration * Time.deltaTime);
    }

    private void RotateLowerTowardsPlayer()
    {
        Vector3 direction = (playerLastSeenPostion - lowerBody.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        lowerBody.rotation = Quaternion.RotateTowards(lowerBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);
    }


    private void ChooseIdleState()
    {
        if (idleStateCooldown <= 0)
        {
            idleStateCooldown = UnityEngine.Random.Range(minIdleStateCooldown, maxIdleStateCooldown);
            currentIdleState = UnityEngine.Random.Range(0, Enum.GetNames(typeof(IdleState)).Length);
            if (currentIdleState == 2) ChooseLeftRightAngle();
        }
        else
        {
            idleStateCooldown -= Time.deltaTime;
        }
    }

    private void ChooseAttackState()
    {
        if (attackStateCooldown <= 0)
        {
            attackStateCooldown = UnityEngine.Random.Range(minAttackStateCooldown, maxAttackStateCooldown);
            currentAttackState = UnityEngine.Random.Range(0, Enum.GetNames(typeof(AttackState)).Length);

            if (currentAttackState == 1) ChooseLeftRightStrave();
        }
        else
        {
            attackStateCooldown -= Time.deltaTime;
        }
    }

    private void ChooseLeftRightAngle()
    {
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            leftRightAngle = 20f;
        }
        else
        {
            leftRightAngle = -20f;
        }
    }

    private void ChooseLeftRightStrave()
    {
        leftRight = UnityEngine.Random.Range(0, 2);
    }





    /// <summary>
    /// Rotated upper body towards the player 
    /// </summary>
    protected override void RotateTowardsPlayer()
    {
        //Rotation of upper body towards player
        Vector3 direction = (playerLastSeenPostion - upperBody.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newDirection = Quaternion.AngleAxis(angle, Vector3.up);
        upperBody.rotation = Quaternion.RotateTowards(upperBody.rotation, newDirection, rotateSmoothness * Time.deltaTime);
    }

    protected override void CheckCanSeePlayer()
    {



        if (player != null)
        {
            playerDirection = (player.transform.position - transform.position).normalized;
        }
        Debug.DrawRay(upperBody.position, playerDirection * detectionRange, Color.red);

        if (Physics.Raycast(upperBody.position, playerDirection, out RaycastHit hitInfo, detectionRange, ~LayerMask.GetMask("Enemy")))
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

    protected override void CheckShouldShoot()
    {
        Vector3 lookDirection = weaponEnd.transform.forward;
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

    protected override void Shoot()
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
                shootCooldown = shootInterval;
                shotCount = 0;
            }

        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0) shootCooldown = 0;
        }
    }
}
