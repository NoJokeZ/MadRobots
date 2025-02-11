public class IceSpikesManager : EnemyManager
{
    protected override void Awake()
    {
        dummyAmount = 0;
        stationaryAmount = 2;
        movingAmount = 3;
        bossAmount = 0;
        base.Awake();
    }
}
