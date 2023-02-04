public class RoboDuck : Bird
{
    private void Awake()
    {
        health = 100;
        speed = 6.0f;
        pointsGain = 5;
        birdType = BirdSpawnProperties.BirdTypes.RoboDuck;
    }
}
