using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour
{
    [Header("待下次解锁的关卡编号")]
    public int nextLevelNumber = 2;

    [Header("锁定的关卡数字颜色设定")]
    public Color lockLevelNumerColor = Color.white;

    public int currentLevalNumber;

    // 关卡选择的背景框
    private Transform selective;

    private void Awake()
    {
        // 选择框对象
        selective = GameObject.FindWithTag("Selective").transform;
    }

    private void Start()
    {
        LevelsInit();
    }

    /// <summary>
    /// 初始化关卡
    /// </summary>
    private void LevelsInit()
    {
        // 为每一个关卡生产数字编号
        int levelCount = transform.childCount;
        for (int i = 0; i < levelCount; i++) {
            Transform child = transform.GetChild(i);
            Text levelText = child.Find("LevelText").GetComponent<Text>();
            levelText.text = (i + 1).ToString();
            // 激活当前已经解锁的关卡
            if (i < nextLevelNumber - 1)
            {
                // 移除‘锁’icon实物
                child.Find("Lock").gameObject.SetActive(false);
            } else
            {

                levelText.color = lockLevelNumerColor;
                // 锁定的关卡不能点击
                child.GetComponent<Button>().interactable = false;
            }
        }
    }

    /// <summary>
    /// 关卡框的点击事件
    /// </summary>
    /// <param name="levelNumber">选中的关卡编号 从0开始</param>
    public void OnLevelButtonClick(int levelNumber)
    {
        // 选中的关卡对象
        Transform levelObject = transform.GetChild(levelNumber);
        // 移动关卡选中效果的作用对象（通过设置父对象达到效果）
        selective.SetParent(levelObject);
        // 未知设置在父对象中心
        selective.localPosition = Vector3.zero;

        // 选择框放在兄弟节点中的最底层， 防止视觉上遮挡其他兄弟节点
        selective.SetSiblingIndex(0);

        currentLevalNumber = levelNumber;

    }

    /// <summary>
    /// 选中关卡并且点击确定按钮
    /// </summary>
    public void OnEnterLevelButtonClick()
    {
        // 存储选中的关卡编号
        LevelSingleton.GetInstance().currentLevelNumber = currentLevalNumber;

        // 切换到游戏场景。
        SceneManager.LoadScene("Star");
    }
}
