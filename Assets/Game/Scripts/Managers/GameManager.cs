using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Mesh meshPlayer;


    [Header("Status Game")]
    public bool isComplete;
    public bool isShakeCamera;
    public bool isVibration;

    [Header("Level Manager")]
    public int levelGame;

    [Header("References")]
    public GameObject youWin;
    public GameObject youLose;
    public GameObject[] maps;
    public TimingBar timingBar;
    public RunDunk RunDunk;
    public BlockGeneration BlockGeneration;
    public Transform trash;
    public Text levelText;
    public RectTransform[] playerUI;
    public Image[] roadBar;

    [Header("Materials")]
    public Material[] m_stack;
    public Material[] m_stickman;

    public Color yellow;
    public Color blue;
    public Color red;

    public string[] nameBot;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        MMVibrationManager.iOSInitializeHaptics();
    }
    
    private void Start()
    {
        GetName();

        levelGame = PlayerPrefs.GetInt("levelGame");
        levelText.text = "Lv." + (levelGame + 1);

        BlockGeneration.GenerateLevel();
        RunDunk.OnStart();
        ActiveMap();
        StartCoroutine(OnEvent_Start());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            LevelUp();
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);

        }
        SetRoad();
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {

    }

    private void GetName()
    {
        nameBot = new string[3];
        nameBot[0] = "You";
        int n1 = Random.Range(0, UIManager.Instance.listName.Length);
        int n2 = Random.Range(0, UIManager.Instance.listName.Length);
        while(n2 == n1)
        {
            n2 = Random.Range(0, UIManager.Instance.listName.Length);
        }
        nameBot[1] = UIManager.Instance.listName[n1];
        nameBot[2] = UIManager.Instance.listName[n2];
    }

    public IEnumerator OnEvent_Start()
    {
        yield return new WaitForSeconds(2.0f);
      //  AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.StartEvent);
    }

    public IEnumerator OnEvent_End()
    {
        yield return new WaitForSeconds(2.0f);
     //   AnalyticsManager.instance.CallEvent(AnalyticsManager.EventType.EndEvent);
    }

    public void SetRoad()
    {
        if (BlockGeneration.FinishZ < 1) return;
        List<float> stt = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            float t = RunDunk.players[i].rigid.transform.position.z / BlockGeneration.FinishZ;
            roadBar[i].fillAmount = t;
            Vector2 p = playerUI[i].anchoredPosition;
            p.x = Mathf.Lerp(-188.0f, 188.0f, t);
            playerUI[i].anchoredPosition = p;
            stt.Add(roadBar[i].fillAmount);
        }

        stt.Sort();

        for(int i = 0;i < stt.Count; i++)
        {
            if(roadBar[i].fillAmount == stt[i])
            {
                roadBar[i].transform.SetAsLastSibling();
                playerUI[i].transform.SetAsLastSibling();
            }
        }
    }

    public void OnClick_RunDunk_StartGame()
    {
        UIManager.Instance.Show_InGameUI();
        RunDunk.OnStartGame();
    }

    private void LevelUp()
    {
        StartCoroutine(OnEvent_End());
        levelGame++;
        PlayerPrefs.SetInt("levelGame", levelGame);
    }

    public void Complete()
    {
        if (isComplete) return;

        isComplete = true;
        StartCoroutine(C_Complete());
    }

    private IEnumerator C_Complete()
    {
        LevelUp();
        youWin.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        youWin.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.Show_CompleteUI();
    }

    public void Fail()
    {
        if (isComplete) return;

        isComplete = true;
        StartCoroutine(C_Fail());
    }

    private IEnumerator C_Fail()
    {
        youLose.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        youLose.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        UIManager.Instance.Show_FailUI();
        yield return null;
    }

    public void ChangeMaterial(Material _c,Material _t)
    {
        StartCoroutine(C_ChangeMaterial(_c, _t));
    }

    private IEnumerator C_ChangeMaterial(Material _c,Material _t)
    {
        float t = 0.0f;

        Color _c_color = _c.GetColor("_Color");
        float _c_metallic = _c.GetFloat("_Metallic");
        float _c_smoothness = _c.GetFloat("_Glossiness");
        Color _c_EmissionColor = _c.GetColor("_EmissionColor");

        Color _t_color = _t.GetColor("_Color");
        float _t_metallic = _t.GetFloat("_Metallic");
        float _t_smoothness = _t.GetFloat("_Glossiness");
        Color _t_EmissionColor = _t.GetColor("_EmissionColor");

        while (t < 1)
        {
            t += Time.deltaTime * 10.5f;

            Color _color = Color.Lerp(_c_color, _t_color, t);
            float _metallic = Mathf.Lerp(_c_metallic, _t_metallic, t);
            float _smoothness = Mathf.Lerp(_c_smoothness, _t_smoothness, t);
            Color _emissionColor = Color.Lerp(_c_EmissionColor, _t_EmissionColor, t);

            _c.SetColor("_Color",_color);
            _c.SetFloat("_Metallic",_metallic);
            _c.SetFloat("_Glossiness",_smoothness);
            _c.SetColor("_EmissionColor",_emissionColor);
            yield return null;
        }
    }

    public void ChangeColor(SpriteRenderer _spr,Color _t)
    {
        StartCoroutine(C_ChangeColor(_spr, _t));
    }

    private IEnumerator C_ChangeColor(SpriteRenderer _spr, Color _t)
    {
        Color c_color = _spr.color;
        _t.a = c_color.a;

        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime * 2.0f;

            _spr.color = Color.Lerp(c_color, _t, t);

            yield return null;
        }
    }

    public void MoveToTrash(GameObject _obj)
    {
        _obj.transform.SetParent(trash.transform);
    }

    public void Vibration()
    {
        if (isVibration) return;

        StartCoroutine(C_Vibration());
    }

    private IEnumerator C_Vibration()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        isVibration = true;
        yield return new WaitForSecondsRealtime(0.2f);
        isVibration = false;
    }

    private int numberPlayerBreak;
    public void PlayerBreak(Vector3 _pos,int _ID)
    {
        GameObject _obj = PoolManager.Instance.GetObject(PoolManager.NameObject.playerBreak);
        ParticleSystemRenderer psr = _obj.GetComponent<ParticleSystemRenderer>();
        psr.material = m_stack[_ID];
        _obj.transform.position = _pos;
        StartCoroutine(C_Active(_obj, 2.0f));
    }

    public void GlassBreak(Vector3 _pos)
    {
        GameObject _obj = PoolManager.Instance.GetObject(PoolManager.NameObject.glassBreak);
        _obj.transform.position = _pos + Vector3.up * 1.0f;
        StartCoroutine(C_Active(_obj, 2.0f));
    }

    private IEnumerator C_Active(GameObject _obj,float _time)
    {
        numberPlayerBreak++;
        _obj.SetActive(true);
        yield return new WaitForSeconds(_time);
        numberPlayerBreak--;
        _obj.SetActive(false);
    }

    public void ActiveMap()
    {
        if(levelGame % 2 == 0)
        {
            maps[0].SetActive(true);
            maps[1].SetActive(false);
        }
        else
        {
            maps[1].SetActive(true);
            maps[0].SetActive(false);
        }
    }

    public void ShakeCamera()
    {
        if (isShakeCamera) return;

        StartCoroutine(C_ShakeCamera());
    }

    private IEnumerator C_ShakeCamera()
    {
        RunDunk.offsetCamera.DOShakeRotation(.6f, 2, 20).SetUpdate(true);
        isShakeCamera = true;
        yield return new WaitForSeconds(0.6f);
        isShakeCamera = false;
    }
}
