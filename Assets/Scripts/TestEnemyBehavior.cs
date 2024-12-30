public class TestEnemyBehavior : EnemyBehavior
{

    protected override void Awake()
    {
        base.Awake();
        HealtPoints = 10;
    }

}
