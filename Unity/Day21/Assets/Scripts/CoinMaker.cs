using UnityEngine;

public class CoinMaker : MonoBehaviour
{
    [Header("金币的预制体")]
    public GameObject coinPrefab;

    [Header("金币生产的时间间隔")]
    public float interval = 1f;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            // 生成金币
            GameObject o
                 = Instantiate(coinPrefab,
                new Vector3(Random.Range(-3, 3), 6, 0),
                Quaternion.identity);

            // 定时销毁
            Destroy(o, 2);
            timer = 0;

        }

    }
}
