using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Transform playerResultParent;
    public PlayerResult playerResultPrefab;
    public PlayerResult myResult;
    public List<PlayerResult> listPlayerResult = new List<PlayerResult>();
    public Text levelBoardText;
    public string[] levelBoardString;
    public Text giftCoin;
    public GameObject buttonContinue;
    public GameObject buttonTextContinue;
    private List<string> listNameRandom = new List<string>();
    private List<Sprite> listFlagRandom = new List<Sprite>();

    public int RankPlayerIndex
    {
        get
        {
            return PlayerPrefs.GetInt("RankPlayerIndex");
        }
        set
        {
            PlayerPrefs.SetInt("RankPlayerIndex", value);
        }
    }

    private int LevelBoard
    {
        get
        {
            return PlayerPrefs.GetInt("LevelBoard");
        }
        set
        {
            PlayerPrefs.SetInt("LevelBoard", value);
        }
    }

    private void OnEnable()
    {
        if(PlayerPrefs.GetInt("RankFirstTime") == 0)
        {
            PlayerPrefs.SetInt("RankFirstTime", 1);
            RankPlayerIndex = 10704;
        }

        ShowResult();
    }

    public void ShowResult()
    {
        if (PlayerPrefs.GetInt("PlayTheFirstTime") == 0)
        {
            SetPlayerSult();
            PlayerPrefs.SetInt("PlayTheFirstTime", 1);
        }

        buttonContinue.SetActive(false);
        buttonTextContinue.SetActive(false);

        if (LevelBoard >= levelBoardString.Length) LevelBoard = levelBoardString.Length - 1;
        levelBoardText.text = levelBoardString[LevelBoard];

        if (listPlayerResult.Count != 0)
        {
            for (int i = 0; i < listPlayerResult.Count; i++)
            {
                Destroy(listPlayerResult[i].gameObject);
            }
        }

        listPlayerResult.Clear();

        playerResultParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

        for(int i = 0; i < 25; i++)
        {
            listFlagRandom.Add(UIManager.Instance.flagsSpr[Random.Range(0, UIManager.Instance.flagsSpr.Length)]);
        }

        int[] p = SimpleMathf.RandomIntArray(25,UIManager.Instance.listName.Length);

        for (int i = 0; i < 25; i++)
        {
            listNameRandom.Add(UIManager.Instance.listName[p[i]]);
        }

        for (int i = 0; i < 25; i++)
        {
            PlayerResult _pr = Instantiate(playerResultPrefab, playerResultParent);
            int _score = (RankPlayerIndex - i) * 2;
            string _name = listNameRandom[i];
            Sprite _flag = listFlagRandom[i];
            _pr.Init(i, _name, _score, _flag, RankPlayerIndex);
            listPlayerResult.Add(_pr);
        }

        gameObject.SetActive(true);

        SetMyResult();
    }

    private void SetMyResult()
    {
        StartCoroutine(C_SetMyResult());
    }

    private IEnumerator C_SetMyResult()
    {
        int myStep = PlayerPrefs.GetInt("myStep");
        myStep = 1;
        if (myStep == 1)
        {
            giftCoin.text = "Gift: " + 100;
            int stt = 18;
            int score = (RankPlayerIndex - stt) * 2;
            int targetStt = Random.Range(5, 8);

            Vector2 targetPos2 = Vector2.up * (stt * 175.0f - 525.0f + (175.0f / 2.0f));

            playerResultParent.GetComponent<RectTransform>().anchoredPosition = Vector2.up * (stt * 175.0f - 525.0f + (175.0f / 2.0f));
            myResult.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -30.0f);

            myResult.Init(stt, "Player", score, UIManager.Instance.flagPlayer,RankPlayerIndex);
            listPlayerResult[stt].HideProfile(false);
            listPlayerResult[targetStt].HideProfile(false);

            Vector2 targetPos = Vector2.up * (targetStt * 175.0f - 475.0f + (175.0f / 2.0f));

            yield return new WaitForSeconds(0.25f);
            myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(0.5f);
            listPlayerResult[stt].HideProfile(true);
            //     coinUI.CoinAnimation(100);
            score = (RankPlayerIndex - stt - targetStt) * 2;
            myResult.Init(targetStt, "Player", score, UIManager.Instance.flagPlayer, RankPlayerIndex);
            RankPlayerIndex -= targetStt;
            playerResultParent.GetComponent<RectTransform>().DOAnchorPos(targetPos, 1.0f).SetEase(Ease.Flash).OnComplete(() => myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo));
        }
     
       
        myStep++;
        if (myStep >= 3)
        {
            LevelBoard++;
            myStep = 0;
            SetPlayerSult();
        }
        PlayerPrefs.SetInt("myStep", myStep);

        yield return new WaitForSeconds(1.2f);
        buttonContinue.SetActive(true);
        buttonTextContinue.SetActive(true);
    }

    public void SetPlayerSult()
    {
        int[] _sps = RandomIntArray(25, 150);

        for (int i = 0; i < _sps.Length; i++)
        {
            PlayerPrefs.SetInt("player_result" + i, _sps[i]);
        }
    }

    private int[] RandomIntArray(int lenght, int maxNumber)
    {
        int[] newArray = new int[lenght];

        for (int i = 0; i < lenght; i++)
        {
            int r = Random.Range(0, maxNumber);

            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    if (newArray[j] == r)
                    {
                        r = Random.Range(0, maxNumber);
                        j = -1;
                    }
                }
            }

            newArray[i] = r;
        }

        return newArray;
    }
}
