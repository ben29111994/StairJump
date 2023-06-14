using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResult : MonoBehaviour
{
    public Text sttText;
    public Text nameText;
    public Text scoreText;
    public Image flagImg;
    public int stt;
    public int stt2;
    public void Init(int _stt, string _name, int _score, Sprite _flag,int _fakeStt)
    {
        stt = (_stt);
        stt2 = _fakeStt;
        sttText.text = (stt2 + stt).ToString();

        if (_name == "Player") _name = "You";
        nameText.text = _name;
        scoreText.text = _score.ToString();
        flagImg.sprite = _flag;
    }

    public void HideProfile(bool isHide)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isHide);
        }
    }
}
