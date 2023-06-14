using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Nhan : MonoBehaviour
{
    public int number;
    public string mathf;

    public TextMeshPro tmp;
    public DouNhan douNhan;


    public void Start()
    {
        if(mathf == "+")
        {
            number = Random.Range(4, 19);
            tmp.text = "+" + number;
        }
    }

    public void Hide()
    {

    }

}
