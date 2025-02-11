using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdle : EnemyState
{

    private bool isTurning;
    private float idleTurnTimer;
    private float idleTurnDelayMin = 5f;
    private float idleTurnDelayMax = 10f;

    private bool isTurretTurning;
    private float idleTurretTurnTimer;
    private float idleTurretTurnDelayMin = 5f;
    private float idleTurretTurnDelayMax = 10f;

    private bool isMoving;
    private float idleMoveTimer;
    private float idleMoveDelayMin = 5f;
    private float idleMoveDelayMax = 10f;

    [SerializeField] EnemyState EnemyIdleState;
    [SerializeField] EnemyState EnemyPlayerInRangeState;


    protected override void Awake()
    {
        base.Awake();

    }


    protected override void Update()
    {
        base.Update();

        IdleTurn();
        IdleTurretTurn();
        IdleMove();
        CheckPlayerInRange();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isMoving)
        {
            velocity = rb.velocity;
        }
    }

    /// <summary>
    /// Decides when to turn
    /// </summary>
    private void IdleTurn()
    {
        if (!isTurning && isWallInFront)
        {
            int direction = Random.Range(0, 2);
            isTurning = true;
            if (direction == 0)
            {
                StartCoroutine(TurnUntilNoWallInFront(Direction.left));
            }
            else
            {
                StartCoroutine(TurnUntilNoWallInFront(Direction.right));
            }
        }
        else if (!isTurning && idleTurnTimer <= 0)
        {
            int direction = Random.Range(0, 2);
            float seconds = Random.Range(1f, 5f);
            isTurning = true;
            if (direction == 0)
            {
                StartCoroutine(TurnForSeconds(Direction.left, seconds));
            }
            else
            {
                StartCoroutine(TurnForSeconds(Direction.right, seconds));
            }
        }
        else
        {
            idleTurnTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Decides when to turn turret
    /// </summary>
    private void IdleTurretTurn()
    {
        if (!isTurretTurning && idleTurretTurnTimer <= 0)
        {
            int direction = Random.Range(0, 2);
            float seconds = Random.Range(1f, 5f);
            isTurretTurning = true;
            if (direction == 0)
            {
                StartCoroutine(TurretTurnForSeconds(Direction.left, seconds));
            }
            else
            {
                StartCoroutine(TurretTurnForSeconds(Direction.right, seconds));
            }
        }
        else
        {
            idleTurretTurnTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Decides when to move
    /// </summary>
    private void IdleMove()
    {
        if(!isMoving && !isWallInFront && idleMoveTimer <= 0)
        {
            int direction = Random.Range(0, 2);
            float seconds = Random.Range(1f, 5f);
            isMoving = true;
            if(direction == 0)
            {
                StartCoroutine(MoveForSeconds(Direction.forward, seconds));
            }
            else
            {
                StartCoroutine(MoveForSeconds(Direction.backward, seconds));
            }
        }
        else
        {
            idleMoveTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Rotates the enemy until there is no wall in front of it anymore
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator TurnUntilNoWallInFront(Direction direction)
    {
        while (isWallInFront)
        {
            RotateBottom(direction);
            yield return null;
        }

        idleTurnTimer = Random.Range(idleTurnDelayMin, idleTurnDelayMax);
        isTurning = false;
    }

    /// <summary>
    /// Rotates the enemy for the specified time
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator TurnForSeconds(Direction direction, float seconds)
    {
        float turnTimer = seconds;
        while (turnTimer >= 0)
        {
            RotateBottom(direction);
            turnTimer -= Time.deltaTime;
            yield return null;
        }

        idleTurnTimer = Random.Range(idleTurnDelayMin, idleTurnDelayMax);
        isTurning = false;
    }

    /// <summary>
    /// Rotates the turret for the specified time
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator TurretTurnForSeconds(Direction direction, float seconds)
    {
        float turretTurnTimer = seconds;
        while (turretTurnTimer >= 0)
        {
            RotateTop(direction);
            turretTurnTimer -= Time.deltaTime;
            yield return null;
        }

        idleTurnTimer = Random.Range(idleTurretTurnDelayMin, idleTurretTurnDelayMax);
        isTurretTurning = false;
    }

    /// <summary>
    /// Moves the enemy for the enemy for specified time
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator MoveForSeconds(Direction direction, float seconds)
    {
        float moveTimer = seconds;
        while (moveTimer >= 0)
        {
            Move(direction);
            moveTimer -= Time.deltaTime;
            if (direction == Direction.forward && isWallInFront)
            {
                break;
            }
            else if (direction == Direction.backward && isWallBehind)
            {
                break;
            }

            yield return null;
        }

        idleMoveTimer = Random.Range(idleMoveDelayMin, idleMoveDelayMax);
        isMoving = false;
    }


    private void CheckPlayerInRange()
    {
        playerDirection = (player.transform.position - upperBody.position).normalized;

        if (Physics.Raycast(upperBody.position, playerDirection, out RaycastHit hitInfo, detectionRange, ~LayerMask.GetMask("Enemy")))
        {
            if (hitInfo.transform.gameObject.layer == playerLayerMaskIndex)
            {
                EnemyPlayerInRangeState.enabled = true;
                EnemyIdleState.enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
