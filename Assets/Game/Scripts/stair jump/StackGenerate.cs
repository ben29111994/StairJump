using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGenerate : MonoBehaviour
{
    public float distance;
    public void Start()
    {
        AutoGenerate();
    }

    private void AutoGenerate()
    {
        float currentZ = transform.position.z - distance;
        float targetZ = transform.position.z + distance;
        float space = 0.6f;
        int count = 3;
        int n = 0;
        int percent = Random.Range(5, 12);
        int[] number = SimpleMathf.RandomIntArray(count);

        for(float z = currentZ; z < targetZ; z += space)
        {
            Vector3 _pos = Vector3.zero;
            _pos.z = z;


            for(int k = 0; k < number.Length; k++)
            {
                _pos.x = (float)(k - 1) * 2.5f;
                _pos.y = transform.position.y;

                Stack _stack = PoolStack.Instance.GetStackInPool();
                _stack.Active(null, number[k], _pos);
            }

            // suffle lane
            n++;
            if(n % percent == 0)
            {
                number = SimpleMathf.RandomIntArray(count);
            }
        }

    }


}
