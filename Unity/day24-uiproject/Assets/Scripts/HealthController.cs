using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    [Header("速度")]
    public float speed = 100;

    private float hor, ver;

    private Text source;
    private Text healthText;
    private Text magicText;
    private RectTransform health;
    private RectTransform magic;
    private Transform healthImg;
    private Transform magicImg;

    private void Awake()
    {
        health = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        magic = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();

        healthText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        magicText = transform.GetChild(1).GetChild(1).GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        health.sizeDelta = new Vector2(
            health.sizeDelta.x + hor * Time.deltaTime * speed,
            health.sizeDelta.y);
        magic.sizeDelta = new Vector2(
            magic.sizeDelta.x + ver * Time.deltaTime * speed,
            magic.sizeDelta.y);

        healthText.text = 100.ToString();
    }
}
