using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Transform[] _spawnPoint;

    private List<int> _spawnOrderByTrack;
    private float[] _spawnTimes;
    private float _minSpawnTime = 0.2f;
    private float _maxSpawnTime = 1f;
    private byte _amountOfBombs = 6;
    private byte _nofSpawnPoints = 3;
    private byte _bombsPerSpawnpoint;

    public static bool SpawnBombs = false;

    private void Start()
    {
        _spawnOrderByTrack = new List<int>();
        _spawnTimes = new float[_amountOfBombs];
        _bombsPerSpawnpoint = (byte)(_amountOfBombs / _nofSpawnPoints);
        
        for (int i=0; i < _spawnTimes.Length; i++)
        {
            _spawnTimes[i] = Random.Range(_minSpawnTime, _maxSpawnTime);
        }

        for (int i=0; i < _nofSpawnPoints; i++)
        {
            for (int j=0; j < _bombsPerSpawnpoint; j++)
            {
                _spawnOrderByTrack.Add(i);
            }
        }

        System.Random rand = new System.Random();
        _spawnOrderByTrack = _spawnOrderByTrack.OrderBy(_ => rand.Next()).ToList();
    }

    public IEnumerator CoSpawnBombs()
    {
        for (int i = 0; i < _spawnTimes.Length; i++)
        {
            yield return new WaitForSeconds(_spawnTimes[i]);

            Instantiate(_bombPrefab, _spawnPoint[_spawnOrderByTrack[i]]);
        }
    }
}
