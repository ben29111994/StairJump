using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{
    public bool isChangeModel;
    public GameObject[] spectatorPrefab;


    private void Start()
    {
        Invoke("OnStart", 1);
    }
    private void OnStart()
    {
        Vector3 dir = Vector3.right;
        dir.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(dir);
        ChangeModel();
    }

    public void ChangeModel()
    {
        isChangeModel = true;
        GameObject m = Instantiate(spectatorPrefab[Random.Range(0, spectatorPrefab.Length)], transform);
        m.transform.localPosition = Vector3.down * 0.4f;
        transform.localScale *= 1.6f;
        GetComponent<Renderer>().enabled = false;
        m.SetActive(true);
    }

    public void ForceCoin()
    {
        //GameObject c = PoolManager.Instance.GetObject(PoolManager.NameObject.Coinspec);
        //c.transform.position = transform.position;
        //c.SetActive(true);
        //Rigidbody rigid = c.GetComponent<Rigidbody>();
        //Vector3 pos = GameManager.Instance.main.transform.position;
        //pos.x += Random.Range(-3.0f, 3.0f);
        //pos.z += Random.Range(-5.0f, 5.0f);
        //Vector3 direction = pos - transform.position;
        //direction.y = direction.magnitude * 0.5f;
        //float force = Random.Range(20.0f, 30.0f);
        //rigid.AddForce(direction * force);
    }

    private IEnumerator C_Hide(GameObject obj)
    {
        yield return new WaitForSeconds(2.0f);
    }
}
