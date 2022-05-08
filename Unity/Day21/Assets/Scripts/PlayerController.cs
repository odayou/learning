using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public int sorce;

    private float hor;
    public float  speed = 3L;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  角色移动
        hor = Input.GetAxis("Horizontal");
        transform.position += Vector3.left * hor * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Coin")
        {
            Destroy(other.gameObject);
            Debug.Log("接到金币" + ++sorce);
        }
    }
}
