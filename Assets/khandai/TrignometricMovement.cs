using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrignometricMovement : MonoBehaviour
{
    public Vector3 Distance;
    public Vector3 MovementFrequency;
    Vector3 FinalPosition;
    Vector3 StartPosition;

    [Header("RandomRange")]
    public bool isRandom;
    public float minDistance;
    public float maxDistance;
    public float minFreq;
    public float maxFreq;

    void Start()
    {
        StartPosition = transform.position;
        if (isRandom)
        {
            Distance = new Vector3(0, Random.Range(minDistance, maxDistance * 0.5f), 0);
            MovementFrequency = new Vector3(0, Random.Range(minFreq, maxFreq * 0.8f), 0);
        }
    }
    void Update()
    {
        FinalPosition.x = StartPosition.x + Mathf.Sin(Time.timeSinceLevelLoad * MovementFrequency.x) * Distance.x;
        FinalPosition.y = StartPosition.y + Mathf.Sin(Time.timeSinceLevelLoad * MovementFrequency.y) * Distance.y;
        FinalPosition.z = StartPosition.z + Mathf.Sin(Time.timeSinceLevelLoad * MovementFrequency.z) * Distance.z;
        transform.position = new Vector3(FinalPosition.x, FinalPosition.y, FinalPosition.z);
    }
}
