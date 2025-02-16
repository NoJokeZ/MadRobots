using UnityEngine;
using UnityEngine.UI;

public class EnemeyHealthBar : MonoBehaviour
{

    private Image healthBarFill;
    private Image healthBar;
    private EnemyBehavior enemyBehaviour;
    private Transform playerTransform;

    private float enemyMaxHealth;

    private float healthBarEnableRange = 30f;

    private bool isHealthBarEnabled;


    private void Awake()
    {
        healthBar = transform.Find("HealthBar").GetComponent<Image>();
        healthBarFill = transform.Find("Fill").GetComponent<Image>();
        enemyBehaviour = gameObject.GetComponentInParent<EnemyBehavior>();
        playerTransform = enemyBehaviour.Player.transform;
    }

    private void Start()
    {
        enemyMaxHealth = enemyBehaviour.EnemyHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckRange();

        if (isHealthBarEnabled)
        {
            UpdateHealthBar();
        }

        transform.rotation = Camera.main.transform.rotation;
    }

    private void UpdateHealthBar()
    {
        float enemyCurrentHealth = enemyBehaviour.EnemyHealth;
        float currentHealthPercentage = enemyCurrentHealth / enemyMaxHealth;

        healthBarFill.fillAmount = currentHealthPercentage;
    }

    private void CheckRange()
    {
        float range = (playerTransform.position - transform.position).magnitude;

        if (range <= healthBarEnableRange)
        {
            healthBarFill.enabled = true;
            healthBar.enabled = true;
            isHealthBarEnabled = true;
        }
        else
        {
            healthBarFill.enabled = false;
            healthBar.enabled = false;
            isHealthBarEnabled = false;
        }
    }
}
