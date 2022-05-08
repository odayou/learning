using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0, 0.05f, 0);
        // float step = 0.8f * Time.deltaTime;
    }
}
