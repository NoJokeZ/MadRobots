using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private Transform playerSpawn;
    private GameObject ChosenPlayerPrefab;
    public bool isPlayerAlive { get; private set; } = false;

    private void Awake()
    {
        playerSpawn = GameObject.Find("PlayerSpawn").transform;
        ChosenPlayerPrefab = Resources.Load<GameObject>("TestPlayer");

    }

    private void Update()
    {
        if (!isPlayerAlive)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(ChosenPlayerPrefab, playerSpawn.position, playerSpawn.rotation);

        isPlayerAlive = true;
    }

    public void OnPlayerDeath()
    {
        isPlayerAlive = false;
    }
}
