using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AmazingAssets.CurvedWorld;
using UnityEngine.UI;

public class RunDunk : MonoBehaviour
{
    [Header("References")]
    public bool is1Bot;
    public bool isTutorial;
    public Text tutorialText;
    public int TutorialStep;
    public GameObject taptoplay;
    public CurvedWorldController CWC;
    public Transform offsetCamera;
    public Transform[] mainCameraT_Array;
    public Player[] players;
    public GameObject[] players3;

    public void Tutorial(bool _isTutorial)
    {
        isTutorial = _isTutorial;

        if (isTutorial == false)
        {
            taptoplay.SetActive(true);
            return;
            
        }
        StartCoroutine(C_Tutorial());
    }

    private IEnumerator C_Tutorial()
    {
        // step 1
        taptoplay.SetActive(false);
        TutorialStep = 0;
        tutorialText.gameObject.SetActive(true);
        tutorialText.text = "Hold To Move";

        // step 2
        yield return new WaitForSeconds(7.2f);
        TutorialStep = 1;
        tutorialText.text = "Release To Create Stair";
        Time.timeScale = 0.0f;
        while (TutorialStep == 1) yield return null;
        Time.timeScale = 1.0f;
        tutorialText.gameObject.SetActive(false);
    }

    public void Win(Player _playerWin)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == _playerWin)
            {
                players[i].Win();
            }
            else
            {
                players[i].Lose();
            }
        }

        for (int i = 0; i < players.Length; i++) if (players[i].isWin) return;
        offsetCamera.transform.DOMove(_playerWin.rigid.transform.position, 1.0f).SetEase(Ease.InOutSine);
    }

    public void OnStart()
    {
        // set lobby camera
        SetMainCameraTransform(mainCameraT_Array[0]);

        is1Bot = GameManager.Instance.levelGame % 2 == 0 ? true : false;
        // set player idle
        for (int i = 0; i < players.Length;i++) players[i].OnStart(i);
        if (is1Bot)
        {
            for (int i = 0; i < players3.Length; i++) players3[i].SetActive(false);
            players[2].Hide();
        }
    }

    public void OnStartGame()
    {
        // set camera ingame running
        SetMainCameraTransform(mainCameraT_Array[1], 1.6f);

        // set player run
        float _time = 1.2f;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].OnStartGame(0.0f);
            _time -= 0.6f;
        }

        bool _isTutorial = GameManager.Instance.levelGame == 0 ? true : false;
        Tutorial(_isTutorial);
    }

    private void SetMainCameraTransform(Transform _t)
    {
        Camera.main.transform.localPosition = _t.localPosition;
        Camera.main.transform.localRotation = _t.localRotation;
    }

    public void SetMainCameraTransform(Transform _t,float _time)
    {
        Camera.main.transform.DOLocalMove(_t.localPosition, _time).SetEase(Ease.InOutSine);
        Camera.main.transform.DOLocalRotate(_t.localEulerAngles, _time).SetEase(Ease.InOutSine);
    }
}
