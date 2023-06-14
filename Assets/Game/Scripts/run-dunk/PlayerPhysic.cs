using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysic : MonoBehaviour
{
    public LayerMask layerStack;
    private Player player;
    private int hitWallIndex;

    private void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    private IEnumerator C_SetHitWall()
    {
        yield return new WaitForSeconds(3.0f);
        hitWallIndex = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            StopAllCoroutines();
            hitWallIndex++;
            StartCoroutine(C_SetHitWall());
            player.HitWall();

            if(hitWallIndex >= 3)
            {
                player.Fail();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack"))
        {
            Stack _stack = other.GetComponent<Stack>();
            if (_stack.NumberID == player.ID || _stack.isCollect)
            {
                if (player.ID == 0) GameManager.Instance.Vibration();
                player.AddStack(_stack);
            }
        }
        else if (other.CompareTag("Water"))
        {
            player.Fail();
        }
        else if (other.CompareTag("Trigger_EndGame_1"))
        {
            GameManager.Instance.Vibration();
            player.EndGame_1();
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (other.CompareTag("Nhan"))
        {
            Nhan _nhan = other.transform.parent.GetComponent<Nhan>();
            if (_nhan.douNhan.IsCollect[player.ID]) return;

            string _m = _nhan.mathf;
            if(_m == "+")
            {
                int number = _nhan.number;
                player.AddStack(number);
            }
            else
            {
                int number = player.listStack.Count;
                player.AddStack(number);
            }

            _nhan.douNhan.IsCollect[player.ID] = true;
        }
        else if (other.CompareTag("TriggerEnd"))
        {
            player.isTriggerEnd = true;
        }
        //else if (other.CompareTag("Item_CongTru"))
        //{
        //    GameManager.Instance.Vibration();
        //    Item_CongTru item_CongTru = other.gameObject.GetComponent<Item_CongTru>();
        //}
        //else if (other.CompareTag("Item_CongTru"))
        //{
        //    GameManager.Instance.Vibration();
        //    Item_CongTru item_CongTru = other.gameObject.GetComponent<Item_CongTru>();
        //}
        //else if (other.CompareTag("Trigger_EndGame_1"))
        //{
        //    GameManager.Instance.Vibration();
        //    player.EndGame_1();
        //}
        //else if (other.CompareTag("Trigger_EndGame_2"))
        //{
        //    GameManager.Instance.Vibration();
        //    player.EndGame_2();
        //}
    }
}
