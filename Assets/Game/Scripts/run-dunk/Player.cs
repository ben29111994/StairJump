using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using AmazingAssets.CurvedWorld;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    [Header("Status")]
    public string myName;
    public int ID;
    public bool isBot;
    public bool isWin;
    public bool isLose;
    public bool isStart;
    public bool isTouchDown;
    public bool isJumpStack;
    public bool isEndGame_1;
    public bool isEndGame_2;
    public bool isDead;
    public bool isJumpOutGround;
    public bool isGrounded;
    public bool isHitObstacle;
    public bool isBigYoyoEffect;
    public bool isTriggerEnd;
    public StatePlayer statePlayer;
    public StateAnim stateAnim;
    public TypeColor typeColor;

    public int memberNumber;
    private int MemberNumber
    {
        get
        {
            return memberNumber;
        }
        set
        {
            memberNumber = value;
            numberText.text = memberNumber.ToString();
        }
    }

    [Header("References")]
    public TextMeshPro nameTMP;
    public Transform CWCpoint;
    public SwipeControl swipeControl;
    public GameObject stackSlot;
    public Animator anim;
    public SkinnedMeshRenderer smr;
    public Rigidbody rigid;
    public GameObject numberObject;
    public TextMesh numberText;
    public Transform cubeHorizontal;
    public LayerMask layerGround;
    public LayerMask layerStack;
    public LayerMask layerObstacle;

    [Header("Stack Input")]
    public Transform stackSlotPrefab;
    public List<Stack> listStack = new List<Stack>();
    private List<Transform> listStackSlot = new List<Transform>();

    [Header("AI")]
    public float moveSpeedAI;
    public Transform AI_Object;
    public Transform[] AI_PointsArray;
    private Stack targetStackAI;
    public float TargetXAI;

    public enum TypeColor
    {
        Blue,
        Red,
        Yellow
    }

    public enum StatePlayer
    {
        idle,
        run,
        jump,
        hit,
        dance
    }

    public enum StateAnim
    {
        idle,
        run,
        falling
    }

    private void Awake()
    {
        GenerateStackSlot();
    }

    void Update()
    {
        if(isBot == false) swipeControl.UpdateStep();
    }

    private void FixedUpdate()
    {
        if (isStart == false || isDead || isWin) return;

        if (statePlayer == StatePlayer.run || statePlayer == StatePlayer.hit)
        {
            Vector3 vel = rigid.velocity;
            vel.z = (statePlayer == StatePlayer.run) ? moveSpeed : vel.z;
            rigid.velocity = vel;

            Vector3 pos = rigid.transform.position;
            pos.x = Mathf.Lerp(pos.x, cubeHorizontal.transform.position.x, 12.0f * Time.deltaTime);
            rigid.MovePosition(pos);
        }
        else if(statePlayer == StatePlayer.jump)
        {
            Vector3 vel = Vector3.zero;
            rigid.velocity = vel;
        }

        UpdateJumpStack();
        if (isJumpStack == false)
        {
            UpdateModelTransform();
            UpdateMemberNumberText();
            UpdateStack();
            CameraFollowPlayer();
        }
    }

    private void LateUpdate()
    {
        if(isWin || isLose)
        {
            UpdateModelTransform();
            UpdateMemberNumberText();
        }

        if (isStart == false || isDead || isWin) return;

        UpdateFalling();
        isGrounded = IsGrounded();
    }

    public void GenerateStackSlot()
    {
        for(int i = 0; i < 1; i++)
        {
            Transform t = Instantiate(stackSlotPrefab, stackSlot.transform);
            t.transform.localPosition = Vector3.forward * (i * t.transform.localScale.z);
            t.gameObject.SetActive(false);
            listStackSlot.Add(t);
        }
    }

    public void OnStart(int _id)
    {
        ID = _id;
        if (ID != 0) isBot = true;

        if(ID == 0)
        {
            typeColor = TypeColor.Yellow;
        }
        else if(ID == 1)
        {
            typeColor = TypeColor.Blue;
        }
        else if(ID == 2)
        {
            typeColor = TypeColor.Red;
        }

        myName = GameManager.Instance.nameBot[_id];
        nameTMP.text = myName;
        smr.material = GameManager.Instance.m_stickman[ID];
        anim.SetTrigger("Idle");
        statePlayer = StatePlayer.idle;
    }

    public void OnStartGame(float _timeDelay)
    {
        if(gameObject.activeSelf) StartCoroutine(C_OnStartGame(_timeDelay));
    }

    private IEnumerator C_OnStartGame(float _timeDelay)
    {
        yield return new WaitForSeconds(_timeDelay);
        isStart = true;
        statePlayer = StatePlayer.run;
        anim.SetTrigger("Run");
        if (isBot)
        {
            StartCoroutine(C_AI());
            StartCoroutine(C_MoveSpeedAI());
        }


        yield return new WaitForSeconds(1.0f);
        UpdateMemberNumberText();
        numberObject.SetActive(true);
    }

    private Stack GetStackInMe()
    {
        if (listStack.Count == 0) return null;
        Stack _stack = listStack[listStack.Count - 1];
        listStack.Remove(_stack);
        return _stack;
    }

    private void SetRun()
    {
        statePlayer = StatePlayer.run;
        rigid.useGravity = true;
        rigid.isKinematic = false;
    }

    private IEnumerator C_MoveSpeedAI()
    {
        while (true)
        {
            moveSpeedAI = Random.Range(2.5f, 5.0f);
            float timeDelay = Random.Range(1.0f, 4.0f);
            yield return new WaitForSeconds(timeDelay);
        }
    }

    private IEnumerator C_AI()
    {
        int overStackIndex = Random.Range(50, 70);
        if (GameManager.Instance.levelGame % 2 == 0) overStackIndex = (int)(overStackIndex * 1.3f);
        bool isOverStack = false;

        while (true)
        {
            // swipe to collect stack
            float _x = StackX();

            if(IsDetectObstacle() || IsDetectObstacleUnder(_x))
            {
                _x = PosXNoneObstacle();
            }

            swipeControl.UpdateBall(_x);
            TargetXAI = _x;



            // AI jump stack
            while (true)
            {
                if (statePlayer == StatePlayer.hit) break;

                // set force
                if (listStack.Count > overStackIndex) isOverStack = true;
                if (isOverStack && listStack.Count == (int)(overStackIndex / 2.0f)) isOverStack = false;
                bool isForce = (listStack.Count > 0 && isOverStack) ? true : false;

                int lvl = GameManager.Instance.levelGame;
                bool hitObstacle = isHitObstacle && lvl >= 0;
                if (IsDetectObstacle() || isForce || hitObstacle || isTriggerEnd)
                {
                    isTouchDown = false;
                }
                else
                {
                    SetRun();
                    isTouchDown = true;
                }

                if (isLose || GameManager.Instance.RunDunk.players[0].isWin) isTouchDown = false;

                if (isTouchDown || isJumpStack) break;

                Stack _stack = GetStackInMe();
                if (_stack == null)
                {
                    SetRun();
                    break;
                }

                StartCoroutine(C_JumpStack(_stack));
                break;
            }
            yield return null;
        }
    }

    public bool IsDetectObstacle()
    {
        Ray ray = new Ray(rigid.transform.position, Vector3.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 5.0f + ID * 3,layerObstacle))
        {
            if (hit.collider != null) return true;

        }

        ray = new Ray(rigid.transform.position + Vector3.forward * 4.0f, Vector3.down);
        if (Physics.Raycast(ray, out hit, 60.0f, layerObstacle))
        {
            if (hit.collider != null) return true;

        }

        return false;
    }

    public float PosXNoneObstacle()
    {
        for(int x = -2;x <= 2; x++)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 2.0f, Vector3.down);
            RaycastHit hit;

            if (Physics.SphereCast(ray, 0.5f, out hit, 200.0f, layerObstacle))
            {

            }
            else
            {
                return x;
            }
         
        }
        return 0.0f;
    }

    public bool IsDetectObstacleUnder()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 2.0f, Vector3.down);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.5f, out hit, 200.0f, layerObstacle))
        {
            if (hit.collider != null) return true;
        }

        return false;
    }

    public bool IsDetectObstacleUnder(float x)
    {
        Vector3 pos = transform.position;
        pos.x = x;
        Ray ray = new Ray(pos + Vector3.up * 2.0f, Vector3.down);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.5f, out hit, 200.0f, layerObstacle))
        {
            if (hit.collider != null) return true;
        }

        return false;
    }

    public float StackX()
    {
        if (IsDetectObstacle())
        {
            targetStackAI = null;
            return rigid.transform.position.x;
        }

        if(targetStackAI != null)
        {
            // remove when AI move over position stack
            float z = targetStackAI.transform.position.z - rigid.transform.position.z;
            if (z < -1.0f)
            {
                targetStackAI = null;
            }
        }

        if(targetStackAI == null)
        {
            Stack _stack = null;
            for (int i = 0; i < AI_PointsArray.Length; i++)
            {
                Vector3 origin = AI_PointsArray[i].position - Vector3.up * 0.4f;
                Vector3 direction = Vector3.forward;
                Ray ray = new Ray(origin, direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 14.0f + ID * 3, layerStack))
                {
                    Stack _s = hit.collider.gameObject.GetComponent<Stack>();
                    if (_s.NumberID == ID)
                    {
                        _stack = _s;
                        break;
                    }
                }
            }

            targetStackAI = _stack;
            if(targetStackAI != null) return targetStackAI.transform.position.x;
        }
        else
        {
            return targetStackAI.transform.position.x;
        }

        return rigid.transform.position.x;
    }

    public void UpdateJumpStack()
    {
        if (isBot) return;
        if (statePlayer == StatePlayer.hit) return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            SetRun();
            isTouchDown = true;
        }
        else
        {
            isTouchDown = false;
        }

        if (isTouchDown || isJumpStack) return;

        Stack _stack = GetStackInMe();
        if (_stack == null)
        {
            SetRun();
            return;
        }

        StartCoroutine(C_JumpStack(_stack));
    }

    private IEnumerator C_JumpStack(Stack _stack)
    {
        isJumpStack = true;
        statePlayer = StatePlayer.jump;
        Vector3 _pos = anim.transform.position;
        _pos.z += _stack.rend.transform.localScale.z;
        _pos.y += _stack.rend.transform.localScale.y;
        _stack.ActivePhysics(_pos);

        rigid.useGravity = false;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        float timeMove = 0.0f;
        float timeLerp = 0.04f;
        Vector3 _posA = rigid.transform.position;
        Vector3 _posB = _pos + Vector3.up * 0.5f;

        while(timeMove < timeLerp)
        {
            float speed = (isBot) ? 0.01f : 0.02f;
            if (isBot)
            {
                float d = GameManager.Instance.RunDunk.players[0].rigid.transform.position.z - rigid.transform.position.z;
                if (d > 4.0f) speed = 0.02f;
            }
            timeMove += speed;

            Vector3 _posC = Vector3.Lerp(_posA, _posB, timeMove / timeLerp);
            rigid.transform.position = _posC;
            UpdateModelTransform();
            UpdateMemberNumberText();
            UpdateStack();
            CameraFollowPlayer();

            if (timeMove < timeLerp) yield return null;
            if (statePlayer == StatePlayer.hit)
            {
                isJumpStack = false;
                yield break;
            }
        }
        isJumpStack = false;
    }

    public void UpdateMemberNumberText()
    {
        numberText.text = memberNumber.ToString();
    }

    private void UpdateModelTransform()
    {
        anim.transform.position = rigid.transform.position - Vector3.up * 0.5f;
        stackSlot.transform.position = anim.transform.position + Vector3.up * 0.1f + Vector3.up * -0.01f * ID + Vector3.forward * 0.75f;

        Vector3 pos = AI_Object.position;
        pos.x = 0.0f;
        pos.y = rigid.transform.position.y;
        pos.z = rigid.transform.position.z;
        AI_Object.position = pos;
    }

    private void UpdateStack()
    {
        if(listStack.Count > 0) listStack[0].transform.position = listStackSlot[0].position;
        for(int i = 1; i < listStack.Count; i++)
        {
            Vector3 a = listStack[i].transform.position;
            Vector3 b = listStack[i - 1].transform.position;
            Vector3 c = Vector3.Lerp(a, b, Time.deltaTime * 30.0f);
            c.z = 0.5f + listStack[i - 1].transform.position.z;
            c.y = listStackSlot[0].position.y;
            listStack[i].UpdateFollowPlayer(c);
        }
    }

    private void CameraFollowPlayer()
    {
        if (isWin || isLose) return;
        if (isBot) return;
        bool isCurveWorld = false;

        if (isCurveWorld)
        {
            CurvedWorldController CWC = GameManager.Instance.RunDunk.CWC;
            Transform offsetCamera = GameManager.Instance.RunDunk.offsetCamera;
            CWCpoint.transform.position = new Vector3(0.0f, anim.transform.position.y, anim.transform.position.z);
            Vector3 tarPos = CWC.TransformPosition(CWCpoint.position);
            offsetCamera.position = Vector3.Lerp(offsetCamera.position, tarPos, 8.0f * Time.deltaTime);
            Quaternion tarRot = CWC.TransformRotation(anim.transform.position, anim.transform.forward, anim.transform.right);
            offsetCamera.rotation = Quaternion.Lerp(offsetCamera.rotation, tarRot, Time.deltaTime * 8.0f);
        }
        else
        {
            Transform offsetCamera = GameManager.Instance.RunDunk.offsetCamera;
            Vector3 tarPos = new Vector3(0.0f, anim.transform.position.y, anim.transform.position.z);
            offsetCamera.position = Vector3.Lerp(offsetCamera.position, tarPos, 8.0f * Time.deltaTime);
        }
       
    }

    public void AddStack(Stack _stack)
    {
        if (isWin) return;
        if (_stack.isDrop) return;

        listStack.Add(_stack);
        _stack.JoinPlayer(this);
        ResetTargetStackAI(_stack);
        BigYoyoEffect();
    }

    public void AddStack(int _number)
    {
        if (isWin) return;

        for(int i = 0; i < _number; i++)
        {
            Stack _stack = PoolStack.Instance.GetStackInPool();
            Vector3 pos = (listStack.Count > 0) ? listStack[listStack.Count - 1].transform.position : listStackSlot[0].position;
            _stack.Active(null, 0, pos);
            AddStack(_stack);
        }
    }

    public void ResetTargetStackAI(Stack _stack)
    {
        if (targetStackAI == null) return;
        targetStackAI = null;
    }

    private void UpdateFalling()
    {
        if (statePlayer == StatePlayer.hit) return;

        if (IsGrounded())
        {
            if (stateAnim != StateAnim.run)
            {
                anim.SetTrigger("Run");
                stateAnim = StateAnim.run;
            }
        }
        else
        {
            if (stateAnim != StateAnim.falling)
            {
                anim.SetTrigger("Falling");
                stateAnim = StateAnim.falling;
            }
        }
    }

    private bool IsGrounded()
    {
        Vector3 _origin = rigid.transform.position;
        Ray ray = new Ray(_origin, Vector3.down);
        bool _isGrounded = Physics.SphereCast(ray, 0.1f, 0.55f, layerGround);
        return _isGrounded;
    }

    public void Fail()
    {
        StartCoroutine(C_Fail());
    }

    private IEnumerator C_Fail()
    {
        if (isDead) yield break;

        while(listStack.Count > 0)
        {
            listStack[0].Hide();
            listStack.RemoveAt(0);
        }

        isDead = true;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        GameManager.Instance.Vibration();
        GameManager.Instance.PlayerBreak(rigid.transform.position + Vector3.up ,ID);
        if(isBot == false) GameManager.Instance.Fail();
        gameObject.SetActive(false);
    }
    
    public void HitWall()
    {
        StartCoroutine(C_HitWall());
    }

    private IEnumerator C_HitWall()
    {
        if (statePlayer == StatePlayer.hit) yield break;

        rigid.useGravity = true;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        Vector3 dir = Vector3.zero;
        dir.y = 1.0f;
        dir.z = -1.0f;
        rigid.AddForce(dir * 500.0f);
        statePlayer = StatePlayer.hit;
        yield return new WaitForSeconds(0.8f);
        statePlayer = StatePlayer.run;
    }

    public void Win()
    {
        isWin = true;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.isKinematic = false;
        rigid.useGravity = true;
        rigid.AddForce(Vector3.down * 400.0f);
        anim.transform.DORotate(Vector3.up * 180.0f, 0.6f);
        nameTMP.transform.DOLocalRotate(Vector3.up * 180.0f, 0.6f);
        statePlayer = StatePlayer.dance;
        if (isLose)
        {
            anim.SetTrigger("Idle");
        }
        else
        {
            Dance();
        }

        if (isBot)
        {
            GameManager.Instance.Fail();
        }
        else
        {
            GameManager.Instance.Complete();
        }
    }

    public void EndGame_1()
    {
        if (isEndGame_1) return;

        isEndGame_1 = true;
        GameManager.Instance.RunDunk.Win(this);
      //  StartCoroutine(C_EndGame_1());
    }

    public void Dance()
    {
        anim.SetTrigger("Dance");
    }

    public void Lose()
    {
        if (isBot)
        {
            isLose = true;
        }
        else
        {
            isWin = true;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.isKinematic = false;
            rigid.useGravity = true;
            rigid.AddForce(Vector3.down * 400.0f);
            anim.transform.DORotate(Vector3.up * 180.0f, 0.6f);
            nameTMP.transform.DOLocalRotate(Vector3.up * 180.0f, 0.6f);
            anim.SetTrigger("Idle");
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void BigYoyoEffect()
    {
        if (isBigYoyoEffect) return;
        StartCoroutine(C_BigYoyoEffect());
    }

    private IEnumerator C_BigYoyoEffect()
    {
        isBigYoyoEffect = true;
        Stack[] _listStack = listStack.ToArray();

        for (int i = _listStack.Length - 1; i >= 0; i--)
        {
            _listStack[i].BigYoyo();
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        isBigYoyoEffect = false;
    }

    public void HitObstacle()
    {
        if (C2_HitObstacle != null) StopCoroutine(C2_HitObstacle);
        C2_HitObstacle = C_HitObstacle();
        StartCoroutine(C2_HitObstacle);
    }

    private IEnumerator C2_HitObstacle;
    private IEnumerator C_HitObstacle()
    {
        isHitObstacle = true;
        yield return new WaitForSeconds(2.0f);
        isHitObstacle = false;
    }
}