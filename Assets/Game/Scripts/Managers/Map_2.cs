using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_2 : MonoBehaviour
{
    public GameObject[] trees_Prefab;

    public void Start()
    {
        for(int i = 0; i < 1000;i+= Random.Range(15,22))
        {
            Vector3 _pos = new Vector3(Random.Range(-15,-20), 0.0f, (float)i * Random.Range(0.9f, 1.2f));
            GenerateTree(_pos);
            _pos = new Vector3(Random.Range(15, 20), 0.0f, (float)i * Random.Range(0.8f,1.2f));
            GenerateTree(_pos);
        }
    }

    public void GenerateTree(Vector3 _pos)
    {
        GameObject _tree = Instantiate(trees_Prefab[Random.Range(0, trees_Prefab.Length)], transform);
        _tree.transform.localPosition = _pos;
        _tree.transform.localScale = Vector3.one * Random.Range(3.5f, 5.5f);
    }
}
