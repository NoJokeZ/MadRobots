using UnityEngine;

public class EnemyTurretV1 : EnemyBehaviour
{

    protected override void Awake()
    {
        base.Awake();
        EnemyType = EnemyType.Stationary;
        Projectile = Resources.Load<GameObject>("Projectile/EnemyProjectile1");
        enemyStats = Resources.Load<EnemyStats>("EnemyStats/Normal");
        GetStats();
    }

    protected override void Update()
    {
        base.Update();
    }
}
