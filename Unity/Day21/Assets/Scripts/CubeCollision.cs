using UnityEngine;

public class CubeCollision : MonoBehaviour
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
    /// 当前物体与碰撞体产生碰撞开始时，调用一次
    /// </summary>
    /// <param name="collision"></param>

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("col" + collision.collider);
        collision.collider.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    /// <summary>
    /// 保持碰撞期间会一直调用， 每帧调用一次
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay" + collision.collider);

    }

    /// <summary>
    ///  与发生碰撞的碰撞体分开时，调用一次。 既，碰撞结束时
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("exit", collision.collider);
        collision.collider.GetComponent<MeshRenderer>().material.color = Color.blue;

    }

}
