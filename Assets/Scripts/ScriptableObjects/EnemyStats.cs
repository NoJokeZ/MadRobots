using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    //Enemy Values
    //Enemy health
    public float EnemyHealth;
    public int EnemyHealthRegeneration;

    //Enemy armor
    public int EnemyBulletArmorPoints;
    public int EnemyExplosionArmorPoints;

    //Enemy movement
    public float EnemyMoveSpeed;
    public float EnemyGroundAcceleration;
    public float EnemyAirAcceleration;

    //Enemy damage
    public float EnemyProjectileDamage;
    public float EnemyMissleDamage;
    public float EnemyExplosionDamage;
    public float EnemyExplosionRadius;

    //Enemy firerate
    public float EnemyFirerate;
    public int BurstShots;

    //Player detection values
    public float DetectionRange;
    public float StartShootAngle;

    //Enemy turning values
    public float RotateSmoothness;
    public float RotateWeaponSmothness;

}
