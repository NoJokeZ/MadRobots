using System.Collections;
using UnityEngine;

public class EnemyPlayerInRange : EnemyState
{
    //All states
    [SerializeField] EnemyState EnemyIdleState;
    [SerializeField] EnemyState EnemyPlayerInRangeState;

    //Additional decision values
    private bool isMoving;
    private float playerOutOfSightTimer;
    private float playerOutOfSightWaitTime = 5f;
    private bool isLowHealth = false;
    private float followDistance = 20f;
    private float backOffDistance = 10f;

    protected override void Awake()
    {
        base.Awake();

        this.enabled = false; //Enemy start in idle -> this disable
    }


    protected override void Update()
    {
        base.Update();

        //Player detection
        CheckPlayer();

        //Bottom rotation and movement
        if (playerDetected)
        {
            CheckShoot();
            CheckMove();
            RotateBottomTowardsPlayer();
        }

        //Top and weapon rotation
        if (playerOnceSeen)
        {
            RotateTopAndWeaponTowardsPlayer();
        }

        //Shooting
        if (startShoot)
        {
            BurstShoot();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //Velocity needs to be apllied seperatly if enemy is not moving or it will still move if it don't wants to move
        if (!isMoving)
        {
            velocity = rb.velocity;
        }
    }

    /// <summary>
    /// A coroutine that runs every second
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OneSecUpdate()
    {
        while (true)
        {
            CheckHealth(); //Checks its own health if should flee or follow

            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Checks if the enemy can see the player
    /// </summary>
    private void CheckPlayer()
    {
        playerDirection = (player.transform.position - upperBody.position).normalized;

        if (Physics.Raycast(upperBody.position, playerDirection, out RaycastHit hitInfo, detectionRange, ~LayerMask.GetMask("Enemy")))
        {
            if (hitInfo.transform.gameObject.layer == playerLayerMaskIndex)
            {
                playerDetected = true;
                playerOnceSeen = true;
                playerLastSeenPostion = hitInfo.transform.position; //Saves the position where the player is seen or was last seen
                playerOutOfSightTimer = playerOutOfSightWaitTime; //Timer to still continue action if player gets out of sight
            }
            else //If no player is hit by ray
            {
                playerDetected = false;
                startShoot = false;
                playerOutOfSightTimer -= Time.deltaTime; //Countdown for outof sight continuing

                if (playerOutOfSightTimer < 0)
                {
                    EnemyIdleState.enabled = true; //Enable idle
                    EnemyPlayerInRangeState.enabled = false; //Disable player in range
                }
            }
        }
        else //If nothing is hit / player out of range
        {
            playerDetected = false;
            startShoot = false;
            playerOutOfSightTimer -= Time.deltaTime;//Countdown for outof sight continuing

            if (playerOutOfSightTimer < 0)
            {
                EnemyIdleState.enabled = true; //Enable idle
                EnemyPlayerInRangeState.enabled = false; //Disable player in range
            }
        }
    }


    /// <summary>
    /// Checks if the enemy should shoot based on where the barrel points
    /// </summary>
    private void CheckShoot()
    {
        Vector3 lookDirection = weaponEnd.transform.forward; //Direction of barrel
        float angle = Vector3.Angle(lookDirection, playerDirection); //angle diffrence between player direction and direction of barrel

        if (angle <= startShootAngle) //If the angle is small enough the enemy starts to shoot
        {
            startShoot = true;
        }
        else
        {
            startShoot = false;
        }
    }

    /// <summary>
    /// Checks if the enemy should move
    /// </summary>
    private void CheckMove()
    {
        float distanceToPlayer = (playerLastSeenPostion - transform.position).magnitude;

        if (!isLowHealth && distanceToPlayer > followDistance) //If high health and far away move forward
        {
            isMoving = true;
            Move(Direction.forward);
        }
        else if (isLowHealth || distanceToPlayer < backOffDistance) //If low health or to close move away
        {
            isMoving = true;
            Move(Direction.backward);
        }
        else //Else don't move
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// Checks own health, sets low health if below half
    /// </summary>
    private void CheckHealth()
    {
        if ((enemyBehaviour.EnemyMaxHealth / enemyBehaviour.EnemyHealth) > 0.5f)
        {
            isLowHealth = true;
        }
        else
        {
            isLowHealth = false;
        }
    }
}
