using UnityEngine;
using UnityEngine.UI;

public class EnemeyHealthBar : MonoBehaviour
{
    //Images
    private Image healthBarFill;
    private Image healthBar;

    //Enemy scripts for health value
    private EnemyBehaviour enemyBehaviour;
    private float enemyMaxHealth;

    //Player transform for alignment
    private Transform playerTransform;

    //Visualization values
    private float healthBarEnableRange = 30f;
    private bool isHealthBarEnabled;


    private void Awake()
    {
        healthBar = transform.Find("HealthBar").GetComponent<Image>();
        healthBarFill = transform.Find("Fill").GetComponent<Image>();
        enemyBehaviour = gameObject.GetComponentInParent<EnemyBehaviour>();
        playerTransform = enemyBehaviour.Player.transform;
    }

    private void Start()
    {
        enemyMaxHealth = enemyBehaviour.EnemyHealth;
    }

    private void Update()
    {
        CheckRange();

        if (isHealthBarEnabled)
        {
            UpdateHealthBar();
        }

        transform.rotation = Camera.main.transform.rotation;
    }

    /// <summary>
    /// Updated the healthbar fill
    /// </summary>
    private void UpdateHealthBar()
    {
        float enemyCurrentHealth = enemyBehaviour.EnemyHealth;
        float currentHealthPercentage = enemyCurrentHealth / enemyMaxHealth;

        healthBarFill.fillAmount = currentHealthPercentage;
    }

    /// <summary>
    /// Checks the range to the player and only enables healthbar when player close enough
    /// </summary>
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
