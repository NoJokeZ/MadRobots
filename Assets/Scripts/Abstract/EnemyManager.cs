using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyManager : MonoBehaviour
{
    //Manager
    protected GameManager gameManager;

    //Enemy amounts on map
    protected int dummyAmount;
    protected int stationaryAmount;
    protected int movingAmount;
    protected int bossAmount;

    //Enemy spawn lists
    protected List<Transform> dummySpawns;
    protected List<Transform> stationarySpawns;
    protected List<Transform> movingSpawns;
    protected List<Transform> bossSpawns;

    //Enemy prefab lists
    protected GameObject[] dummy;
    protected GameObject[] stationary;
    protected GameObject[] moving;
    protected GameObject[] boss;

    //Map enemy count
    protected int enemyCount;

    //Spawn logics
    protected bool isInitiating;
    protected bool allEnemiesSpawned = false;

    protected virtual void Awake()
    {
        isInitiating = true;

        gameManager = GameManager.Instance;

        GetSpawns();
        GetEnemies();
    }

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        if (isInitiating)
        {
            CheckInitialization();
        }
    }

    /// <summary>
    /// Gets all spawn points on the map
    /// </summary>
    private void GetSpawns()
    {
        dummySpawns = transform.Find("Dummy").GetComponentsInChildren<Transform>().ToList<Transform>();
        stationarySpawns = transform.Find("Stationary").GetComponentsInChildren<Transform>().ToList<Transform>();
        movingSpawns = transform.Find("Moving").GetComponentsInChildren<Transform>().ToList<Transform>();
        bossSpawns = transform.Find("Boss").GetComponentsInChildren<Transform>().ToList<Transform>();
    }

    /// <summary>
    /// Gets all enemy prefabs
    /// </summary>
    private void GetEnemies()
    {
        dummy = Resources.LoadAll<GameObject>("Enemy/Dummy/");
        stationary = Resources.LoadAll<GameObject>("Enemy/Stationary/");
        moving = Resources.LoadAll<GameObject>("Enemy/Moving/");
        boss = Resources.LoadAll<GameObject>("Enemy/Boss");
    }

    /// <summary>
    /// Spawns all enemies
    /// </summary>
    private void SpawnEnemies()
    {
        //Dummy
        if (dummyAmount > 0)
        {
            while (dummyAmount > 0)
            {
                //Get random SpawnPoint and random enemy
                int spawnPoint = Random.Range(1, dummySpawns.Count);
                int enemy = Random.Range(0, dummy.Length);

                //Spawn that
                Instantiate(dummy[enemy], dummySpawns[spawnPoint].position, Quaternion.identity);

                //Count amount down
                dummyAmount--;

                //Add to enemy count
                enemyCount++;
            }
        }

        //Stationary
        if (stationaryAmount > 0)
        {
            while (stationaryAmount > 0)
            {
                //Get random SpawnPoint and random enemy
                int spawnPoint = Random.Range(1, stationarySpawns.Count);
                int enemy = Random.Range(0, stationary.Length);

                //Spawn that
                Instantiate(stationary[enemy], stationarySpawns[spawnPoint].position, Quaternion.identity);

                //Count amount down
                stationaryAmount--;

                //Add to enemy count
                enemyCount++;
            }
        }

        //Moving
        if (movingAmount > 0)
        {
            while (movingAmount > 0)
            {
                //Get random SpawnPoint and random enemy
                int spawnPoint = Random.Range(1, movingSpawns.Count);
                int enemy = Random.Range(0, moving.Length);

                //Spawn that
                Instantiate(moving[enemy], movingSpawns[spawnPoint].position, Quaternion.identity);

                //Count amount down
                movingAmount--;

                //Add to enemy count
                enemyCount++;
            }
        }

        //Boss
        if (bossAmount > 0)
        {
            while (bossAmount > 0)
            {
                //Get random SpawnPoint and random enemy
                int spawnPoint = Random.Range(1, bossSpawns.Count);
                int enemy = Random.Range(0, boss.Length);

                //Spawn that
                Instantiate(boss[enemy], bossSpawns[spawnPoint].position, Quaternion.identity);

                //Count amount down
                bossAmount--;

                //Add to enemy count
                enemyCount++;
            }
        }

        allEnemiesSpawned = true;
    }

    /// <summary>
    /// Checks if initialization is finished and starts the coroutinge for enemy count
    /// </summary>
    private void CheckInitialization()
    {
        if (allEnemiesSpawned)
        {
            isInitiating = false;
            StartCoroutine(CheckEnemyCountCO());
        }
    }

    /// <summary>
    /// Checks only every second if all enemies are killed
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckEnemyCountCO()
    {
        while (enemyCount > 0)
        {
            yield return new WaitForSeconds(1f);
        }
        gameManager.LevelEnd();
    }

    /// <summary>
    /// Counts down the enemy count
    /// </summary>
    public void EnemyDied()
    {
        enemyCount--;
    }

}
