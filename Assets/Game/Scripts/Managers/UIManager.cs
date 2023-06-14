using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public string[] listName;
    public Sprite[] flagsSpr;
    public Sprite flagPlayer;

    [Header("References")]
    public GameObject Lobby;
    public GameObject Ingame;
    public GameObject Win;
    public GameObject Lose;
    public GameObject Loading;

    [Header("Camera Test")]
    public Transform[] cameraArray;
    public Transform cameraMain;
    public int cameraIndex;

    private void Start()
    {
        StartCoroutine(C_Loading());
    }

    private IEnumerator C_Loading()
    {
        Loading.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        Loading.SetActive(false);
    }

    public void Show_MainMenuUI()
    {
        Lobby.SetActive(true);
    }

    public void Show_InGameUI()
    {
        Lobby.SetActive(false);
        Ingame.SetActive(true);
    }

    public void Show_CompleteUI()
    {
        Win.SetActive(true);
    }

    public void Show_FailUI()
    {
        Lose.SetActive(true);
    }

    public void OnClick_LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClick_ChangeCamera()
    {
        cameraIndex++;
        if (cameraIndex >= cameraArray.Length) cameraIndex = 0;

        cameraMain.localPosition = cameraArray[cameraIndex].localPosition;
        cameraMain.localEulerAngles = cameraArray[cameraIndex].localEulerAngles;
    }
}
