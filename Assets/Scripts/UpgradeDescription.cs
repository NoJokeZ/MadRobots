using TMPro;
using UnityEngine;

public class UpgradeDescription : MonoBehaviour
{
    private TextMeshProUGUI description;
    private Transform playerTransform;

    private float descriptionShowRange = 8f;


    private void Awake()
    {
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckRange();

        transform.rotation = Camera.main.transform.rotation;
    }


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
