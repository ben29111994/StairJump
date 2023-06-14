using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Item_CongTru : MonoBehaviour
{
    public float distance;

    [Header("References")]
    public SphereCollider spCollider;
    public GameObject item;
    public TextMesh numberText;
    public MeshRenderer numerFrame;

    public enum TypeCongTru
    {
        cong,
        tru
    }
    // mac dinh : "xanh la +"     "vang la -"

    private void Update()
    {
       
    }
}
