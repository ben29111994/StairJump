using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnimationObtacle : MonoBehaviour
{
    public bool isFreeze;

    public float timeDelay;
    private Animator anitor;

    public void Start()
    {
        anitor = GetComponent<Animator>();
        anitor.updateMode = AnimatorUpdateMode.AnimatePhysics;
        StartCoroutine(C_Start());
    }

    private void Update()
    {
        UpdateFreeze();
    }

    private IEnumerator C_Start()
    {
        anitor.speed = 0.0f;
        yield return new WaitForSeconds(1.0f);
        yield return new WaitForSeconds(timeDelay);
        anitor.speed = 1.0f;
    }

    private void UpdateFreeze()
    {
        if (isFreeze) return;

        float _z = transform.position.z - GameManager.Instance.RunDunk.players[0].rigid.transform.position.z;

        if(_z <= -1.0f)
        {
            isFreeze = true;
            anitor.speed = 0.0f;
            StopAllCoroutines();
        }

        if (_z < -12.0f) gameObject.SetActive(false);
    }
}
