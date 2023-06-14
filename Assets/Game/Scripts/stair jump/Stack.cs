using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stack : MonoBehaviour
{
    [Header("Status")]
    public int NumberID;
    public bool isDrop;
    public bool isCollect;
    public bool isBigYoyo;
    public bool isTEst;

    [Header("References")]
    public BoxCollider col;
    public Rigidbody rigid;
    public Renderer rend;
    public Player player;

    public void JoinPlayer(Player _player)
    {
        gameObject.layer = 14;
        player = _player;
        NumberID = player.ID;
        rend.material = GameManager.Instance.m_stack[NumberID];
        transform.eulerAngles = Vector3.zero;
    }

    public void Active(Player _player,int _ID,Vector3 _pos)
    {
        NumberID = _ID;
        player = _player;
        gameObject.layer = 13;
        isCollect = false;
        rend.material = GameManager.Instance.m_stack[NumberID];
        col.isTrigger = true;
        transform.position = _pos;
        gameObject.SetActive(true);

        Invoke("CheckHideBot", 0.1f);
    }

    public void CheckHideBot()
    {
        if (GameManager.Instance.RunDunk.is1Bot)
        {
            if (NumberID == 2)
            {
                isTEst = true;

                Hide();
            }

        }
    }

    public void Hide()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.isKinematic = true;
        rigid.useGravity = false;

        transform.eulerAngles = Vector3.zero;
        transform.position = Vector3.zero;
        player = null;
        isDrop = false;
        isCollect = false;
        isBigYoyo = false;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public void ActivePhysics(Vector3 _pos)
    {
        transform.position = _pos;
        Drop();
    }

    public void UpdateFollowPlayer(Vector3 _pos)
    {
        transform.position = _pos;
    }

    public void Drop()
    {
        if (isDrop) return;
        if(gameObject.activeSelf) StartCoroutine(C_Drop());
    }

    private IEnumerator C_Drop()
    {
        isDrop = true;
        player = null;
        yield return new WaitForSeconds(1.0f);
        col.isTrigger = false;
        rigid.isKinematic = false;
        rigid.useGravity = true;
        rigid.AddForce(Vector3.down * 300.0f);
        yield return new WaitForSeconds(0.2f);
        while (rigid.velocity.sqrMagnitude > 0.01f || rigid.angularVelocity.sqrMagnitude > 0.01f) yield return null;

        rend.material = GameManager.Instance.m_stack[3];
        gameObject.layer = 13;
        col.isTrigger = true;
        rigid.isKinematic = true;
        rigid.useGravity = false;
        isCollect = true;
        isDrop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (player != null)
            {
                player.listStack.Remove(this);
                player.HitObstacle();
            }
            GameManager.Instance.PlayerBreak(transform.position - Vector3.forward, NumberID);
            StopAllCoroutines();
            Hide();
        }
        else if (other.CompareTag("TriggerEnd"))
        {
            if (player != null)
            {
                player.isTriggerEnd = true;
            }
        }
    }

    public void BigYoyo()
    {
        if (isBigYoyo || gameObject.activeSelf == false) return;
        StartCoroutine(C_BigYoyo());
    }

    private IEnumerator C_BigYoyo()
    {
        isBigYoyo = true;
        transform.DOScale(Vector3.one * 1.4f, 0.08f).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(0.16f);
        isBigYoyo = false;
    }
}