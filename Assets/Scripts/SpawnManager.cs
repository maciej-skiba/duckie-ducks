using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// TODO: How SpawnManager works
    /// </summary>

    // e.g. Spawn 2 ducks for 1 eagle


    [SerializeField] private GameObject[] _spawners;
    [SerializeField] private GameObject _duckPrefab;
    [SerializeField] private GameObject _roboDuckPrefab;
    [SerializeField] private GameObject _eaglePrefab;
    [SerializeField] private GameObject _roboEaglePrefab;

    private Dictionary<string, int> _birdTypeProportionRatios;
    private Dictionary<string, int> _birdTypeAmounts;

    private List<BirdSpawnProperties> _birdSpawnProperties;
    private List<BirdSpawnProperties> _shuffledBirdSpawnProperties;
    private int _nofSpawners;
    private int _nofBirdsToSpawn;
    private float _spawnRate;

    private void Awake()
    {
        switch (RoundEnd.currentLevel)
        {
            case (1):
                _spawnRate = 0.5f;
                break;
            case (2):
                _spawnRate = 0.4f;
                break;
            case (3):
                _spawnRate = 0.3f;
                break;
            default:
                Debug.Log("Unknown LevelManager.currentLevel");
                break;
        }
        _nofSpawners = _spawners.Length;
        _nofBirdsToSpawn = (int)(Timer.timeLeft / _spawnRate) * 2; // *2 to make sure there will be enough birds at the end of timer
        _birdSpawnProperties = new List<BirdSpawnProperties>();
    }
    private void Start()
    {
        switch (RoundEnd.currentLevel)
        {
            case (1):
                _birdTypeProportionRatios = new Dictionary<string, int>()
                {
                    { "Duck", 2 },
                    { "RoboDuck", 2 },
                    { "Eagle", 0 },
                    { "RoboEagle", 0 }
                };
                break;
            case (2):
                _birdTypeProportionRatios = new Dictionary<string, int>()
                {
                    { "Duck", 2 },
                    { "RoboDuck", 2 },
                    { "Eagle", 1 },
                    { "RoboEagle", 1 }
                };
                break;
            case (3):
                _birdTypeProportionRatios = new Dictionary<string, int>()
                {
                    { "Duck", 2 },
                    { "RoboDuck", 2 },
                    { "Eagle", 1 },
                    { "RoboEagle", 1 }
                };
                break;
            default:
                Debug.Log("Unknown LevelManager.currentLevel");
                break;
        }
        PrepareSpawns();
        StartCoroutine(CoSpawnBirds());
    }

    private void PrepareSpawns()
    {
        int sumOfRatios = 0;

        foreach (var ratio in _birdTypeProportionRatios)
        {
            sumOfRatios += ratio.Value;
        }

        // Dictionary values = calculations how many birds of each species should be spawned
        _birdTypeAmounts = new Dictionary<string, int>
        {
            { "Duck", (int)Mathf.Ceil(
                ((float)_birdTypeProportionRatios["Duck"] * (float)_nofBirdsToSpawn) / sumOfRatios) },
            { "RoboDuck", (int)Mathf.Ceil(
                ((float)_birdTypeProportionRatios["RoboDuck"] * (float)_nofBirdsToSpawn) / sumOfRatios) },
            { "Eagle", (int)Mathf.Ceil(
                ((float)_birdTypeProportionRatios["Eagle"] * (float)_nofBirdsToSpawn) / sumOfRatios) },
            { "RoboEagle", (int)Mathf.Ceil(
                ((float)_birdTypeProportionRatios["RoboEagle"] * (float)_nofBirdsToSpawn) / sumOfRatios) }
        };

        // TODELETE, przykład podczas runu, łącznie _birdTypeAmounts jest 32, a _nofBirdsToSpawn 30

        BirdSpawnProperties.BirdTypes currentBirdType = BirdSpawnProperties.BirdTypes.Duck; //default value

        foreach (var amount in _birdTypeAmounts)
        {
            switch(amount.Key)
            {
                case "Duck":
                    currentBirdType = BirdSpawnProperties.BirdTypes.Duck;
                    break;
                case "RoboDuck":
                    currentBirdType = BirdSpawnProperties.BirdTypes.RoboDuck;
                    break;
                case "Eagle":
                    currentBirdType = BirdSpawnProperties.BirdTypes.Eagle;
                    break;
                case "RoboEagle":
                    currentBirdType = BirdSpawnProperties.BirdTypes.RoboEagle;
                    break;
            }
            for (int i=0; i < amount.Value; i++)
            {
                _birdSpawnProperties.Add(new BirdSpawnProperties(_spawnRate, currentBirdType, _nofSpawners));
            }
        }

        System.Random rand = new System.Random();
        _shuffledBirdSpawnProperties = _birdSpawnProperties.OrderBy(_ => rand.Next()).ToList();
    }

    IEnumerator CoSpawnBirds()
    {
        foreach (var bird in _shuffledBirdSpawnProperties)
        {
            yield return new WaitForSeconds(bird.spawnTime);
            
            switch (bird.birdType)
            {
                case BirdSpawnProperties.BirdTypes.Duck:
                    Instantiate(_duckPrefab, _spawners[bird.trackNumber].transform);
                    break;
                case BirdSpawnProperties.BirdTypes.RoboDuck:
                    Instantiate(_roboDuckPrefab, _spawners[bird.trackNumber].transform);
                    break;
                case BirdSpawnProperties.BirdTypes.Eagle:
                    Instantiate(_eaglePrefab, _spawners[bird.trackNumber].transform);
                    break;
                case BirdSpawnProperties.BirdTypes.RoboEagle:
                    Instantiate(_roboEaglePrefab, _spawners[bird.trackNumber].transform);
                    break;
            }
        }
    }
}
