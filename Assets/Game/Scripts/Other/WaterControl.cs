using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material m = GetComponent<Renderer>().material;
        m.SetFloat("_WaterHeight", transform.position.y - 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
