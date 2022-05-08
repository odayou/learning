using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fengz : MonoBehaviour
{
    int[] arr = { 1, -1 };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
  
        float r = Random.Range(0, 3) - 1;
        float step = r * 3f * Time.deltaTime;
        Debug.Log(r + "" + step);
        transform.Translate(0, step, 0, Space.Self);
    }
}
