public class LevelSingleton
{
    private static LevelSingleton instance;
    // 私有构造函数，使得外部不能new此对象
    private LevelSingleton () {}
    public static LevelSingleton GetInstance()
    {
        if (instance == null )
        {
            instance = new LevelSingleton();
        }
        return instance;
    }

    /// <summary>
    /// 当前关卡
    /// </summary>
    public int currentLevelNumber;


}
