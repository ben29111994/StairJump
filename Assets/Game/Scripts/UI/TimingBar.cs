using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour
{
    public GameObject pivot;
    public Text powerText;
    public AnimationCurve ac;

    public bool isTap;

    public void ActiveTimingBar()
    {
        gameObject.SetActive(true);
        StartCoroutine(C_ActiveTimingBar());
    }

    private IEnumerator C_ActiveTimingBar()
    {
        isTap = false;
       // Time.timeScale = 0.06f;

        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.unscaledDeltaTime * 0.5f;
            float e = ac.Evaluate(t);

            Vector3 euler = pivot.transform.eulerAngles;
            euler.z = Mathf.Lerp(-41.5f, 41.5f, e);
            pivot.transform.eulerAngles = euler;

            float powerNumber = Mathf.Lerp(0, 374, e);
            powerText.text = (int)powerNumber + "";

            if (isTap)
            {
                break;
            }

            if (t >= 1.0f) t = 0.0f;

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    public void OnTap(out bool a)
    {
        bool b = false;
        isTap = true;
        float angle = Mathf.Abs(pivot.transform.rotation.z);
        b = (angle > 0.25) ? false : true;
        a = b;
    }
}
