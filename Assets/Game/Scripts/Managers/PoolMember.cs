using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMember : MonoBehaviour
{
    public static PoolMember Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Transform parent;
    public int amount;
    public Member objectPrefab;

    [HideInInspector]
    public List<Member> listObject = new List<Member>();

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        for(int i = 0; i < amount; i++)
        {
            Member objectClone = Instantiate(objectPrefab, parent);
            objectClone.gameObject.SetActive(false);
            listObject.Add(objectClone);
            yield return null;
        }
    }

    public Member GetMemberInPool()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Member childObject = listObject[i];
            if (childObject.gameObject.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Member objectClone = Instantiate(objectPrefab, parent);
        objectClone.gameObject.SetActive(false);
        listObject.Add(objectClone);
        return objectClone;
    }
}
