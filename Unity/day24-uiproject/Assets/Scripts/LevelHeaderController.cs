using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHeaderController : MonoBehaviour
{
    private Text levelHeadText;
    private void Awake()
    {
        levelHeadText = GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        levelHeadText.text = "Level " + (LevelSingleton.GetInstance().currentLevelNumber + 1);
    }
}
