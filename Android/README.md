*此项目是学习android 应用开发的笔记，源码存放在I磁盘 -- 磁盘已损坏*
# 学习资料
- 《第一行代码第2版》
- https://www.imooc.com/learn/96
- https://www.imooc.com/learn/107
- https://www.imooc.com/learn/142
- https://www.imooc.com/course/list?c=android&is_easy=1
# 知识点

## java配置
- jdk java开发工具包
- jre  java执行环境
- java_home java路径
- classpath java工具包路径
- path jre安装路径
- avd 安卓模拟器 android virtual device manager

## 安卓工程结构
- assets 资源目录 ，里面的资源不会自动编译进应用程序，只有用到你的资源才会
- bin  编译后生成的文件
- libs 第三方jar包
- res 资源目录 区别于assets, 里面的所有资源会编译到apk包中
    - drawable 不同分辨率的图片资源
    - layout 布局文件
    - values 常量 如工程字体大小 颜色 （values v11 指对应到drawable的规格）
- androidManisest.xml 引用的权限等，**apk执行的时候要先扫描此文件**
## 控件
- TextView 显示文本框
    - 属性
        - id
        - layout_width 控件的宽度
            - wrap_content 包裹实际内容，实际内容有多宽 就显示多宽
            - match_parent 当前控件铺满父类容器剩余部分 **2.3 api之后添加的属性值 **
            - fill_parent 当前控件铺满父类容器剩余部分 和match-parent完全一样
        - textSize 文本字体 **单位sp**
        - ...
- EditText 输入文本框
    - 属性
        - ...
        - hint 提示文本，未输入内容时候的默认文本
        - inputType 文本框类型
        - text 控件的文本内容， 即输入的值
- ImageView
    - src 图片路迳
    - background 设置背景图片或者颜色
- Button 按钮
- ImageButton 图片按钮
## findViewById 查找控件对象 ，返回父类View对象需要强转为具体的控件对象
## autoCompleteTextView 动态匹配输入的内容
- complationThreshold 输入多少个字符时开始匹配
## MultiaAutoCompleteTextView 动态匹配输入的内容 -多选
- tokenizer 分隔符，输入分隔符带包一次选择完成， 再次输入会内容重新匹配
## toggleButton 选中/未选中切换的按钮
- checked 是否选中 默认 false
- textOff 未选中的文本 默认"off"
- textOn 选中的文本  默认"on"
## checkBox 复选框
- checked 是否选中
- text 展示的文本
## radioButton单选框
## radioGroup单选框集合
- orientation 集合原子的排列方式 vertical 垂直排布 horizontal 水平排布
## activity 活动
*把布局xml文件引入活动使用setContentView(R.layout.main_activity), main_activity是布局xml的文件名*
## R.java
项目app/build/generated/source/r/工程名/... 目录下会生成R.java文件， 包含了res目录下的所有属性的内容,所以通过@string 等能访问到对应的内容
## 监听器
- setOnClickListener 点击事件监听
    - 内部类监听
      setOnClickListener(new OnClickListener(){

})
- 外部类监听 自定义类实现(implements) OnClickListener*接口*, 实现 onClick方法, 推荐使用，便于所有按钮执行通用动作
- 实现接口监听，活动(activity)实现OnClickListener接口，并实现onClick方法
- setOnCheckedChangeListener 选择切换时间监听
## 跑马灯
控件长度超出父控件范围（singleLine=true）时实现滚动展示
- 单个控件 ellipsesize=marquee focusable=true focusableInTouchMode=true 控件获取焦点的时候执行跑马灯效果
- 多个控件 自定义控件,继承TextView 设置isFocused(){return true;}, 使用此控件申明的控件都会获取到焦点，达到多个控件同时执行marquee效果
## 尺寸单位
	px 像素 不能根据分辨率实现等比 不推荐 
	dip dp 控件长度大小显示 新sdk推荐dp
	sp 类似dp 一般用于显示文字
## 布局 布局可以嵌套
- 线性布局 linearLayout   子控件横向或者纵向并排依次排列
- orientation 排序方式 vertical垂直排列 horizontal横向排列
- gravity 子控件在布局中的位置(x,y轴的位置)
    - bottom 底部显示 左上角
    - right
    - center   水平和垂直同时居中
    - bottom|center_horizontal 底部水平居中
    - 类推
- layout_gravity 当前控件的位置
- layout_weight 子控件在父控件中的比例(同辈组件个数均分后占用的比例)
