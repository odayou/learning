using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pappy : MonoBehaviour
{
    private bool left = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(left && transform.position.x > 5)
        {
            left = false;
            transform.localEulerAngles = new Vector3(0, -180, 0);
        }
        if (!left && transform.position.x < -5)
        {
            left = true;
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        float step = 3f * Time.deltaTime;
        transform.Translate(step, 0, 0, Space.Self);
    }
}
