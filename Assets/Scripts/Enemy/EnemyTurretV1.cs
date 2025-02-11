using UnityEngine;

public class EnemyTurretV1 : EnemyBehavior
{

    protected override void Awake()
    {
        base.Awake();
        EnemyType = EnemyType.Stationary;
        Projectile = Resources.Load<GameObject>("EnemyeProjectile1");
        enemyStats = Resources.Load<EnemyStats>("EnemyStats/Normal");
        GetStats();
    }

    protected override void Update()
    {
        base.Update();
    }
}
