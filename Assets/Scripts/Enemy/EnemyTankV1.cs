using UnityEngine;

public class EnemyTankV1 : EnemyBehavior
{

    protected override void Awake()
    {
        base.Awake();
        EnemyType = EnemyType.Moving;
        Projectile = Resources.Load<GameObject>("EnemyeProjectile1");
        enemyStats = Resources.Load<EnemyStats>("EnemyStats/Normal");
        GetStats();
    }

    protected override void Update()
    {
        base.Update();
    }
}
