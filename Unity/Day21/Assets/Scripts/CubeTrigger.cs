using UnityEngine;

public class CubeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 触发开始时调用一次
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("触发开始，" + other);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("触发持续，" + other);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("触发结束，" + other);
    }
}
