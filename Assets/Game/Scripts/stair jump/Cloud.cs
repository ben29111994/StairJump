using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Renderer ground;
    public Renderer[] stroke;
    public Material _mFade;
    public Material _mNoFade;
    public Material _mFade2;
    public Material _mNoFade2;

    public void Fade()
    {
        StopAllCoroutines();
        StartCoroutine(C_Fade());
    }

    private IEnumerator C_Fade()
    {
        ground.material = _mNoFade;
        stroke[0].material = _mNoFade2;
        stroke[1].material = _mNoFade2;
        yield return new WaitForSeconds(0.1f);
        ground.material = _mFade;
        stroke[0].material = _mFade2;
        stroke[1].material = _mFade2;
    }

}
