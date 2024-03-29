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
    private byte _nofSpawnPoints = 3;
    private byte _bombsPerSpawnpoint;

    public static bool SpawnBombs = false;
    public byte amountOfBombs = 6;

    private void Start()
    {
        PrepareBombSpawns();
    }

    public IEnumerator CoSpawnBombs()
    {

        for (int i = 0; i < _spawnTimes.Length; i++)
        {
            yield return new WaitForSeconds(_spawnTimes[i]);

            Instantiate(_bombPrefab, _spawnPoint[_spawnOrderByTrack[i]]);
        }

        if (!BossFight.s_finalStageStarted) StartCoroutine(BossFight.Instance.CoPrepareLastFightStage());
    }

    public void PrepareBombSpawns()
    {
        if (amountOfBombs % 3 != 0) amountOfBombs = (byte)((byte)(amountOfBombs / _nofSpawnPoints) * _nofSpawnPoints);

        _spawnOrderByTrack = new List<int>();
        _spawnTimes = new float[amountOfBombs];
        _bombsPerSpawnpoint = (byte)(amountOfBombs / _nofSpawnPoints);

        for (int i = 0; i < _spawnTimes.Length; i++)
        {
            _spawnTimes[i] = Random.Range(_minSpawnTime, _maxSpawnTime);
        }

        for (int i = 0; i < _nofSpawnPoints; i++)
        {
            for (int j = 0; j < _bombsPerSpawnpoint; j++)
            {
                _spawnOrderByTrack.Add(i);
            }
        }

        System.Random rand = new System.Random();
        _spawnOrderByTrack = _spawnOrderByTrack.OrderBy(_ => rand.Next()).ToList();
    }
}
