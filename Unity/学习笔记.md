# Unity2021零基础入门学习教程 - RollABall

更多教程访问www.sikiedu.com

老师：SiKi



## Unity介绍

Unity的诞生

> https://www.jianshu.com/p/55021d9301ab

Unity干了什么

> https://baike.baidu.com/item/Unity/10793?fr=aladdin

Unity发展

> https://baike.baidu.com/item/Unity/10793?fr=aladdin#3

Unity职业发展和薪资待遇

> 找工作渠道
>
> 拉勾 Boss直聘 脉脉 校园招聘 技术社区（论坛、QQ群）

Unity独立游戏

> 关于独立游戏的定义
>
> 1、小团队开发的游戏
>
> 2、不为了赚钱的而开发的非商业游戏
>
> 3、有个人情怀和故事的游戏

独立游戏制作人（不具有代表性，个别）

> 凌绮梦https://space.bilibili.com/191064174/
>
> 独立游戏故事：https://space.bilibili.com/97249155/
>
> 王妙一https://space.bilibili.com/14032516/
>
> 混沌之神https://space.bilibili.com/85816940/
>
> Alivehttps://space.bilibili.com/580207/
>
> SiKi学院老师：凌绮梦   海洋   凉鞋

## 编程学习方法

1. 跟着老师做练习
2. 通过练习
   1. 需要记住的自然就记住了
   2. 不需要记住了自然就忘记了（用到查-百度-官方文档）

学会自己解决问题

> 百度、bilibili、知乎

### Unity学习顺序

先学习Unity基础还是学习编程基础

第一种方案

C#编程 -> Unity基础 ->Unity进阶

第二种方案

Unity基础 -> C#编程+Unity混合

## 下载和安装

### 下载地址

> Unity中国官网 unity.cn
>
> 国际官网unity.com
>
> 官网：www.unity.com
>
> GetStarted或者网页底部的Get Unity
>
> 个人版和收费版

### 关于网络

> 国际官网打开不是很顺畅，有的时候可以打开有的时候不能打开，打开较慢，
>

### 下载入口分类

#### 新手用户

> （Beta版本不稳定）安装一个固定版本（2018）+示例工程

UnityHubSetup

选择安装目录

登录Unity ID

选择工程

安装进行...

#### 老用户

> 安装任意个任意版本的unity

UnityHubSetup

选择安装目录

运行Unity Hub

选择版本（2021.1.2）一般保持前两个版本一致即可

选择模块（模块后期可以添加）

​	VS

​	Document

​	语言

### 关于版本

> LTS（Long Term Support）长期支持版本
>
> ​	为什么长期支持版本？只更新最新版本不就可以了吗？
>
> Alpha内测版
>
> Beta公测版（不稳定，有最新功能，最新更新）
>
> 关于不同版本的解释：https://blog.csdn.net/zistxym/article/details/99300504

![image-20210411202509967](C:\Users\souke\AppData\Roaming\Typora\typora-user-images\image-20210411202509967.png)

#### 历史版本

首页->底部Download Archive



## Unity基本知识

### 创建工程

什么是工程

工程目录介绍

布局介绍

语言切换（初学者可以使用中文，建议慢慢喜欢英文界面）

### 创建脚本

如何创建C#脚本（Unity支持哪些脚本）

怎么打开脚本

设置VS的字体（工具 -> 选项 -> 字体和颜色）

> JetBrains Mono
>
> https://www.jetbrains.com/zh-cn/lp/mono/

代码不提示的问题

> Edit -> Preference -> External Tools

### 创建场景

场景介绍

怎么创建

### 五个窗口面板（视图、窗口）

**Project**：工程面板，存放工程的各种资源。包括：声音资源、模型资源、场景文件、材质、脚本等。

**Hierarchy**：层级面板，展示当前打开的场景里面有哪些东西。（里面的东西称为游戏物体）

**Inspector**：检视面板（或属性面板），查看一个游戏物体由哪些组件组成。

> 场景 -> 多个游戏物体 -> 多个组件

**Scene**：场景面板，显示当前场景的样子

> 鼠标中键：平移视角
>
> 鼠标右键：旋转视角（环顾四周），围绕自身旋转
>
> Alt+鼠标左键：渲染视角，围绕当前视角前方位置

**Game**：游戏面板，场景运行的时候的样子

### 场景、游戏物体、组件

对应的英文：**Scene**，**GameObject**，**Component**。

场景的简单编辑操作：创建、复制、删除。

### Unity中的基本模型和场景操作

1、如何创建基本模型和如何导入复杂模型

2、场景基本操作

> 聚焦：双击游戏物体 或者 F
>
> 放大缩小视野：鼠标滚轮
>
> 围绕物体旋转：Alt+鼠标左键
>
> 使用MoveTool下 移动物体

3、**视野分类**

> **Persp** 透视视野 	**ISO**平行视野
>
> 在不同视野下：关于鼠标右键的不同

4、**保存**（场景保存、代码保存）

> Ctrl + S

### 世界坐标系和局部坐标系

**坐标系**：x 左右 y 上下 z前后

**局部坐标系**：父物体和子物体

**单位：**Unity里面的坐标是以米为单位的

### 工具栏

**四个工具**:Q W E R

**Q：**视图工具（使用鼠标中键可以临时切换到这个恐惧）

**W：**移动工具

**E：**旋转工具

**R：**缩放工具

**其他工具**

> Rect Tool		Transform Tool		Collider Tool

**切换工具**

> 位置工具：Pivot原点 - Center中心
>
> 局部和全局坐标切换：Local - Global

**步移工具**

> ![image-20210414184114448](C:\Users\souke\AppData\Roaming\Typora\typora-user-images\image-20210414184114448.png)
>
> 按钮
>
> 快捷键：Ctrl
>
> 条件：世界坐标系下

## 案例开发

### 案例演示

### 开发步骤

1、创建工程

2、创建场景

3、创建小球和地面 - Plane  （位置都是0,0,0）

4、创建材质，修改地面颜色

### 材质

游戏物体 - 材质 - 贴图 - 颜色 - Shader（着色器）  这几个关系

### 基本组件

**Transform**：变换组件，位置、旋转、缩放。

**Mesh Filter**：网格

**Meth Render**：网格渲染（这个组件会使用材质进行渲染）

**Collider**：碰撞检测

> 为什么渲染模型和碰撞模型要分开？
>
> -游戏世界和现实世界的区别

### 添加刚体组件

**作用：**模拟物理效果（重力、摩擦力、弹力、动力等），可以通过刚体控制小球的运动。

### 添加代码组件

**代码零基础学生**：模仿 不求甚解。 后期学习C#语法基础

**如何添加代码，如何删除代码？**

> 添加两种方式：1、在Project里面	2，在Inspector面板上
>
> 删除步骤：1、删除代码 2、删除组件

### 写代码 - 控制小球移动

**工具**：在Preference里面指定使用的编辑工具，默认是VS。

### 脚本的基本结构

1、引用

2、脚本名字（类名）（类名和脚本名保持一致）

3、什么是方法

4、Start方法和Update方法 - 系统方法（事件方法）

### 控制台面板 - Console

1、怎么打开

2、怎么输出

3、收缩输出

4、注释

> 几个概念：方法   输出   打印    日志   调用  注释

### 在代码中获取刚体组件

第一种方法 - 通过代码获取

```c#
private Rigidbody rd;

rd = rd = GetComponent<Rigidbody>();
```

第二种方法 - 通过拖拽

```c#
public Rigidbody rd;
```

### 给小球添加力

```c#
rd.AddForce(Vector3.forward);
```

**知识点1：**方法调用 

x.AddForce 也可以叫做调用指令，调用指令的时候需要通过（）传递参数。

小明你去买点菠菜。 小明.买(菠菜)     小明.买菠菜()

**知识点2：**三维向量  (x,y,z) 

Vector3.forward 等于 （0,0,1）

向量两个要素（方向，长度）  （0,0,1）  （0,0,4）

**知识点3：**一些常用的向量

Vector3.right  Vector3.left   Vector3.forward  Vector3.back  Vector3.up Vector3.down

**知识点4：**怎么创建向量

new Vector3(x,y,z)

###  通过按键控制左右运动

```c#
float h = Input.GetAxis("Horizontal");
rd.AddForce(new Vector3(h,0,0));
```

1、 A D 键 和 左右方向键

2、如何设置（Project Setting -》Input Manager）

### 前后（通过上下键）

```c#
float h = Input.GetAxis("Horizontal");
float v = Input.GetAxis("Vertical");
rd.AddForce(new Vector3(h,0, v));
```

### 控制力的大小（速度的大小）

```c#
public int force = 5;
```

### 控制相机位置和跟随

**步骤：**

1、得到Player的Transform

2、计算位置偏移

3、根据位置偏移设置相机的位置

> 额外知识：
>
> 编程中的变量    向量的计算（初高中知识）  类-组件

```c#
    public Transform playerTransform;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTransform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + offset;
    }
```

### 添加墙体

### 创建可捡起物和创建预制体（Prefab）

1、添加Pickup

2、制作预制体

> 为什么做成预制体？
>
> 怎么修改预制体？
>
> 预制体和示例的单向传递（Class Object）

3、让Pickup旋转起来

```c#
transform.Rotate(Vector3.up,Space.World);
```

### 如何查看文档

### 检测碰撞

1、系统事件（消息，方法）

> 系统调用（发送）
>
> 示例：
>
> OnCollisionEnter	OnCollisionExit	OnCollisionStay
>
> 测试碰撞是三个事件

2、获取到碰撞的物体名字

```c#
private void OnCollisionEnter(Collision collision)
{
    string name = collision.collider.name;
    Debug.Log(name);
}
```

> 知识点
>
> GameObject Component
>
> component.gameObject
>
> component.gameObject.name
>
> component.name

3、添加标签，通过标签区分某一类物体（食物、敌人）

```c#
private void OnCollisionEnter(Collision collision)
{
    if(collision.collider.tag == "PickUp")
    {
        Destroy(collision.collider.gameObject);
    }
}
```

> **注意：**书写代码要在英文输入法下。

### 触发检测

1、哪些方法

> OnTriggerEnter OnTriggerStay OnTriggerExit

2、使用

```c#
private void OnTriggerEnter(Collider other)
{
}
```

### 计分

### UI显示分数

```c#
using UnityEngine.UI;

public Text text;

text.text = "分数2";
```

### 添加胜利提示UI

### 游戏发布

1、安装发布模块

2、发布（Build 构建）

