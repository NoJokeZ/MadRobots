using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    //Player Values
    //Player health
    public int PlayerMaxHealth;
    public int PlayerHealthRegeneration;

    //Player armor
    public int PlayerBulletArmorPoints;
    public int PlayerExplosionArmorPoints;

    //Player movement
    public float PlayerMoveSpeed;
    public float PlayerGroundAcceleration;
    public float PlayerAirAcceleration;
    public float PlayerJetPackPower;
    public float PlayerJetPackMaxDuration;

    //Player damage
    public float PlayerProjectileDamage;
    public float PlayerMissleDamage;
    public float PlayerExplosionDamage;
    public float PlayerExplosionRadius;

    //Player firerate
    public float PlayerFirerate;

    //
    public int PlayerSpecialAbilityAmmo;

}
