using TMPro;
using UnityEngine;

public class UpgradeDescription : MonoBehaviour
{
    //Gameobjects
    private TextMeshProUGUI description;
    private Transform playerTransform;

    //Show range
    private float descriptionShowRange = 8f;


    private void Awake()
    {
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        CheckRange();

        transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// Checks range towards the player
    /// </summary>
    private void CheckRange()
    {
        float range = (playerTransform.position - transform.position).magnitude;

        if (range <= descriptionShowRange)
        {
            description.enabled = true;
        }
        else
        {
            description.enabled = false;
        }
    }
}
