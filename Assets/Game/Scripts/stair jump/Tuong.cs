using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuong : MonoBehaviour
{
    void Start()
    {
        int r = Random.Range(1, 4);
        transform.GetChild(r).gameObject.SetActive(true);
    }

}
