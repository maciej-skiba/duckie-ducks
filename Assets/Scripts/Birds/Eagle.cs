public class Eagle : Bird
{
    private void Awake()
    {
        health = 200;
        speed = 8.0f;
        pointsGain = -15;
        birdType = BirdSpawnProperties.BirdTypes.Eagle;
    }
}
