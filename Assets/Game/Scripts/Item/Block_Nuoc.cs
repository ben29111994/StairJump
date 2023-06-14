using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Nuoc : MonoBehaviour
{
    public Transform[] nuocArray;

    public void Start()
    {
        int x = 0;
        int lvl = GameManager.Instance.levelGame;
        if (lvl % 2 == 0)
        {
            x = 1;
        }
        else
        {
            x = 0;
        }

        nuocArray[x].gameObject.SetActive(true);
      //  nuocArray[x].GetChild(x).gameObject.SetActive(true);
    }
}
