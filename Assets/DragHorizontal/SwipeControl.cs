using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    public static SwipeControl Instance;

    private void Awake()
    {
        Instance = (Instance == null) ? this : Instance;
    }

    public Transform fakeBall;
    public Transform fakeBall2;

    public LayerMask layer;
    public Camera cam;
    public bool isHold;
    public Vector3 currentTouchPosition;
    public Vector3 lastTouchPosition;
    public Vector3 deltaTouchPosition;

    public float minPosX = -2.8f;
    public float maxPosX = 2.8f;

    public void UpdateStep()
    {
        UpdateSwipe();
        UpdateBall();
    }

    private void UpdateSwipe()
    {
        bool touchBegan = false;
        bool touchMoved = false;
        bool touchEnded = false;

   

#if UNITY_EDITOR
        touchBegan = Input.GetMouseButtonDown(0);
        touchMoved = Input.GetMouseButton(0);
        touchEnded = Input.GetMouseButtonUp(0);
#else
        if(Input.touchCount > 0)
        {
            touchBegan = Input.touches[0].phase == TouchPhase.Began;
            touchMoved = Input.touches[0].phase == TouchPhase.Moved;
            touchEnded = Input.touches[0].phase == TouchPhase.Ended;
        }
#endif

        if (touchBegan)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit,layer))
            {
                currentTouchPosition = lastTouchPosition = hit.point;
            }

            isHold = true;
        }
        else if (touchMoved)
        {
            if(isHold == false)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, layer))
                {
                    currentTouchPosition = lastTouchPosition = hit.point;
                }

                isHold = true;
            }

            RaycastHit hit2;
            Ray ray2 = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray2, out hit2, layer))
            {
                currentTouchPosition = hit2.point;
            }

            deltaTouchPosition = currentTouchPosition - lastTouchPosition;
            lastTouchPosition = currentTouchPosition;

        }
        else if (touchEnded)
        {
            currentTouchPosition = lastTouchPosition = deltaTouchPosition = Vector3.zero;
            isHold = false;

            if(GameManager.Instance.RunDunk.TutorialStep == 1)
            {
                GameManager.Instance.RunDunk.TutorialStep = 2;
            }
        }
    }

    public void UpdateBall()
    {
        float d = Mathf.Clamp(deltaTouchPosition.x, -0.2f, 0.2f);
        Vector3 v = fakeBall2.position;
        v.x +=  d * 1.4f;
        v.x = Mathf.Clamp(v.x, minPosX, maxPosX);
        fakeBall2.transform.position = v;
        fakeBall.transform.position = v;
    }

    public void UpdateBall(float _x)
    {
        Vector3 v = fakeBall2.position;
        v.x = Mathf.Lerp(v.x, _x, Time.deltaTime * 4.0f);
        fakeBall2.transform.position = v;
        fakeBall.transform.position = v;
    }
}
