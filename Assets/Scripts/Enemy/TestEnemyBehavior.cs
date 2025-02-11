public class TestEnemyBehavior : EnemyBehavior
{

    protected override void Awake()
    {
        base.Awake();
        EnemyHealth = 10;
    }

}
