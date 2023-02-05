using UnityEngine;

public class BirdSpawnProperties
{
    public float spawnTime;
    public BirdTypes birdType;
    public int trackNumber;

    public enum BirdTypes
    {
        Duck,
        RoboDuck,
        Eagle,
        RoboEagle
    }

    public BirdSpawnProperties(float spawnTime, BirdTypes birdType, int nofTracks)
    {
        this.spawnTime = Random.Range(spawnTime - spawnTime / 4, spawnTime + spawnTime / 4);
        this.birdType = birdType;
        this.trackNumber = Random.Range(0, nofTracks);
    }
}
