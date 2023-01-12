using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : Bird
{
    private void Awake()
    {
        health = 100;
        speed = 4.0f;
        pointsGain = 5;
    }
}
