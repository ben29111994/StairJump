using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DouNhan : MonoBehaviour
{
    public bool[] IsCollect;

    public Nhan[] nhanArray;

    private void Start()
    {
        if(Random.value > 0.5f)
        {
            nhanArray[0].transform.localPosition = new Vector3(-1.9f, 0.0f, 0.0f);
            nhanArray[1].transform.localPosition = new Vector3(1.9f, 0.0f, 0.0f);
        }
        else
        {
            nhanArray[1].transform.localPosition = new Vector3(-1.9f, 0.0f, 0.0f);
            nhanArray[0].transform.localPosition = new Vector3(1.9f, 0.0f, 0.0f);
        }
    }
}
