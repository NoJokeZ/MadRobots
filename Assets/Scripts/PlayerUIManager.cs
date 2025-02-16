using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    private Image healthBar;
    private Image fuelBar;

    private PlayerBehavior playerBehaviour;

    private float playerMaxHealth;

    private float playerMaxFuel;

    private void Awake()
    {
        healthBar = transform.Find("HealthBar/Full").GetComponent<Image>();
        fuelBar = transform.Find("Fuel/Full").GetComponent<Image>();
        playerBehaviour = gameObject.GetComponentInParent<PlayerBehavior>();
    }

    private void Start()
    {
        playerMaxHealth = playerBehaviour.HealthPoints;
        playerMaxFuel = playerBehaviour.JetPackMaxDuration;
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateFuelBar();
    }

    private void UpdateHealthBar()
    {
        float playerCurrentHealth = playerBehaviour.HealthPoints;
        float currentHealthPercentage = playerCurrentHealth / playerMaxHealth;

        healthBar.fillAmount = currentHealthPercentage;
    }

    private void UpdateFuelBar()
    {
        float playerCurrentFuel = playerBehaviour.JetPackDuration;
        float currentFuelPercentage = playerCurrentFuel / playerMaxFuel;

        fuelBar.fillAmount = currentFuelPercentage;
    }
}
