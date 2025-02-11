public class BigCityManager : EnemyManager
{
    protected override void Awake()
    {
        dummyAmount = 0;
        stationaryAmount = 3;
        movingAmount = 3;
        bossAmount = 0;
        base.Awake();
    }
}
