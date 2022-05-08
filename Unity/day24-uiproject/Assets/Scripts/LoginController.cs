using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    private InputField userName;
    private InputField passWord;
    private Button loginButton;

    private void Awake()
    {
        userName = transform.GetChild(0).GetComponent<InputField>();
        passWord = transform.GetChild(1).GetComponent<InputField>();
        loginButton = transform.GetChild(2).GetComponent<Button>();
    }
// Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(OnLogonNButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLogonNButtonClick ()
    {
        Debug.Log("用户名:" + userName.text + "密码:" + passWord.text);
    }
}
