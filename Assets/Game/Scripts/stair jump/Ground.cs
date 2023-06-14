using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject multiGroundPrefab;
    public GameObject singleMesh;
    private List<GameObject> _listMultiGround = new List<GameObject>();

    private void Start()
    {
        bool isCurvedWorld = false;

        if (isCurvedWorld)
        {
            int amount = (int)transform.localScale.z;
            transform.localScale = Vector3.one;
            GameObject _multiMeshParent = new GameObject();
            _multiMeshParent.transform.SetParent(transform);
            _multiMeshParent.transform.localPosition = Vector3.forward * (-amount / 2.0f + 0.5f);
            _multiMeshParent.name = "MultiMesh";

            for (int i = 0; i < amount; i++)
            {
                GameObject _obj = Instantiate(multiGroundPrefab);
                _obj.transform.SetParent(_multiMeshParent.transform);
                _obj.transform.localPosition = Vector3.forward * i;
            }

            singleMesh.SetActive(false);
            singleMesh.transform.localScale = new Vector3(1.0f, 1.0f, amount);
        }
        else
        {

        }
    }
}
