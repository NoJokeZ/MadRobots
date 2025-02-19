using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    //Images
    private Image healthBar;
    private Image fuelBar;

    //Player scripts
    private PlayerBehaviour playerBehaviour;

    //Values
    private float playerMaxHealth;
    private float playerMaxFuel;

    private void Awake()
    {
        //Get images and player script
        healthBar = transform.Find("HealthBar/Full").GetComponent<Image>();
        fuelBar = transform.Find("Fuel/Full").GetComponent<Image>();
        playerBehaviour = gameObject.GetComponentInParent<PlayerBehaviour>();
    }

    private void Start()
    {
        //Get the max values
        playerMaxHealth = playerBehaviour.HealthPoints;
        playerMaxFuel = playerBehaviour.JetPackMaxDuration;
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateFuelBar();
    }

    /// <summary>
    /// Updates the healthbar
    /// </summary>
    private void UpdateHealthBar()
    {
        float playerCurrentHealth = playerBehaviour.HealthPoints;
        float currentHealthPercentage = playerCurrentHealth / playerMaxHealth;

        healthBar.fillAmount = currentHealthPercentage;
    }

    /// <summary>
    /// Updates the fuelbar
    /// </summary>
    private void UpdateFuelBar()
    {
        float playerCurrentFuel = playerBehaviour.JetPackDuration;
        float currentFuelPercentage = playerCurrentFuel / playerMaxFuel;

        fuelBar.fillAmount = currentFuelPercentage;
    }
}
