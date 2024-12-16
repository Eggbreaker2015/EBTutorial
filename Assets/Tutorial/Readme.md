# Tutorial 系统使用说明文档

## 1. 系统概述
Tutorial 系统是一个用于实现游戏内新手引导的功能模块，提供了高亮显示、箭头指示、强制引导等功能，可以帮助玩家快速了解游戏操作流程。

## 2. 核心功能
- 高亮显示UI元素
- 可配置的引导箭头
- 强制/非强制引导模式
- 支持按钮和Toggle组件的点击事件
- 进度保存与加载
- 条件触发系统

## 3. 配置教程数据

### 3.1 TutorialData 结构
```csharp
public class TutorialData
{
    public int TutorialId;        // 教程ID
    public bool IsForce;          // 是否强制引导
    public Condition[] Conditions; // 触发条件
    public List<StepData> Steps;  // 引导步骤
}
```

### 3.2 配置步骤数据 (StepData)
```csharp
public class StepData
{
    public string ViewName;      // UI界面名称
    public int Step;             // 步骤序号
    public string Desc;          // 描述文本
    public string highLightPath; // 高亮对象路径
    public string ArrowTarget;   // 箭头指向目标
    public int ArrowDir;         // 箭头方向
    public Vector2 ArrowOffset;  // 箭头偏移
    public bool isClickBtn;      // 是否需要点击高亮按钮 如果为false 则点击任意位置进入下一步
}
```

## 4. 使用方法

### 4.1 创建教程配置
1. 在 Unity 编辑器中创建 TutorialSO 数据, 或者通过Excel配置数据
2. 配置教程数据，包括步骤信息、条件等
3. 确保引导目标的路径配置正确

### 4.2 初始化教程系统

tutorialCanvas 需要实例化并且挂载到场景中。res目录下有默认的tutorialCanvas Prefab

```csharp
// 初始化Manager
TutorialManager.Instance.Initialize(tutorialSO.TutorialDatas, tutorialService, tutorialService, tutorialCanvas);

// 项目中的UI框架打开和关闭事件需要调用TutorialManager的OnOpenUIView和OnCloseUIView方法
// 例如：
private void OnUIOpen(UIBase base)
{
    TutorialManager.Instance.OnOpenUIView(base.name, base.gameObject);
}

private void OnUIClose(UIBase base)
{
    TutorialManager.Instance.OnCloseUIView(base.name);
}
```

## 5. 高级功能

### 5.1 条件系统
可以设置触发教程的条件：
```csharp
public class Condition
{
    public string Key;                 // 条件键
    public float Value;               // 目标值
    public ComparisonOperator Operator; // 比较运算符
}
```
继承ITutorialCondition接口，实现CheckCondition方法
```csharp
public class TutorialService : ITutorialCondition
{
    public bool CheckCondition(TutorialData.Condition[] conditions)
    {
        //TODO: 检查条件
        return true;
    }
}
```

### 5.2 进度保存
使用 TutorialProgress 类管理教程进度：
```csharp
public class TutorialProgress
{
    public int TutorialId;    // 教程ID
    public int CurrentStep;   // 当前步骤
    public bool IsCompleted;  // 是否完成
}
```

继承ITutorialStorage接口，实现SaveProgress和LoadProgress方法
```csharp
public class TutorialService : ITutorialStorage
{
    public void SaveProgress(List<TutorialProgress> progressList)
    {
        //TODO: 保存进度
    }
}
```

## 6. 注意事项
1. 确保目标UI对象路径配置正确
2. 强制引导模式下会显示遮罩层
3. 高亮对象需要有正确的Canvas设置
4. 箭头位置可能需要根据实际UI布局调整


## 7. 待完成内容
1. 箭头显示控制
2. 非强制引导功能完善
3. 教程跳过功能
4. 步骤描述弹窗
