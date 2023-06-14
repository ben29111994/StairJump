using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPlayer : MonoBehaviour
{
    public Stack stack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack"))
        {
            Stack _stack = other.GetComponent<Stack>();
            if (_stack == stack) return;
            if (_stack.player != null) return;
            Player player = stack.player;
            if (player == null) return;

            if (_stack.NumberID == player.ID || _stack.isCollect)
            {
                if (player.ID == 0) GameManager.Instance.Vibration();
                player.AddStack(_stack);
            }
        }
        else if (other.CompareTag("Nhan"))
        {
            Nhan _nhan = other.transform.parent.GetComponent<Nhan>();
            Player player = stack.player;
            if (player == null) return;

            if (_nhan.douNhan.IsCollect[player.ID]) return;

            string _m = _nhan.mathf;
            if (_m == "+")
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
    }
}
