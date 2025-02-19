using UnityEngine;

public class EnemyStationary : EnemyState
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayer();

        if (playerDetected)
        {
            RotateTopAndWeaponTowardsPlayer();
            CheckShoot();
        }

        if (startShoot)
        {
            Shoot();
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// <summary>
    /// Checks if the enemy can see the player
    /// </summary>
    private void CheckPlayer()
    {
        playerDirection = (player.transform.position - upperBody.position).normalized; //player direction
        if (Physics.Raycast(upperBody.position, playerDirection, out RaycastHit hitInfo, detectionRange, ~LayerMask.GetMask("Enemy"))) //Raycast that ignores if it hits itself
        {
            if (hitInfo.transform.gameObject.layer == playerLayerMaskIndex) //Check if player is hit
            {
                playerDetected = true;
                playerOnceSeen = true;
                playerLastSeenPostion = hitInfo.transform.position; //Saves the position where the player is seen or was last seen
            }
            else //Stops if no player is hit
            {
                playerDetected = false;
                startShoot = false;
            }
        }
        else //Stops if no player is out of range
        {
            playerDetected = false;
            startShoot = false;
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


}
