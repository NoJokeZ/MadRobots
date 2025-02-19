public class TutorialManager : EnemyManager
{
    protected override void Awake()
    {
        dummyAmount = 0;
        stationaryAmount = 0;
        movingAmount = 0;
        bossAmount = 0;
        base.Awake();
    }
}
