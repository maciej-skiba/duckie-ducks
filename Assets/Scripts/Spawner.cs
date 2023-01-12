using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject bird;

    private void Start()
    {
        InvokeRepeating("SpawnBird", 1, 2);
    }

    private void SpawnBird()
    {
        Instantiate(bird, this.transform.position, transform.rotation);
    }
}
