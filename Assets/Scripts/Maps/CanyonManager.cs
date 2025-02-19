public class CanyonManager : EnemyManager
{
    protected override void Awake()
    {
        dummyAmount = 0;
        stationaryAmount = 4;
        movingAmount = 1;
        bossAmount = 0;
        base.Awake();
    }
}
