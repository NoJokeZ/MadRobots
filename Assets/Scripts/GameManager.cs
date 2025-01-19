using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private Transform playerSpawn;
    private GameObject ChosenPlayerPrefab;
    public bool IsPlayerAlive { get; private set; } = false;

    private void Awake()
    {
        playerSpawn = GameObject.Find("PlayerSpawn").transform;
        ChosenPlayerPrefab = Resources.Load<GameObject>("RocketV1");

    }

    private void Update()
    {
        if (!IsPlayerAlive)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(ChosenPlayerPrefab, playerSpawn.position, playerSpawn.rotation);

        IsPlayerAlive = true;
    }

    public void OnPlayerDeath()
    {
        IsPlayerAlive = false;
    }
}
