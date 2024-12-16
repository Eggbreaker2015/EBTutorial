using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public abstract class UIBase : MonoBehaviour
{
    public string UIPath { get; set; }
    public UIType UIType;
    public UILayer UILayer;
    
    protected List<UnityEngine.Object> registeredEvents = new List<UnityEngine.Object>();
    
    public event Action OnOpenComplete;
    public event Action OnCloseComplete;
    
    protected virtual void Awake()
    {
        InitComponents();
    }

    protected virtual void InitComponents()
    {
        // 子类重写以初始化组件引用
    }

    public virtual async Task OnOpenAsync()
    {
        gameObject.SetActive(true);
        // 子类重写以实现打开动画等
        await Task.CompletedTask;
    }

    public virtual void OnClose()
    {
        // 子类重写以实现关闭动画等
    }

    protected virtual void OnDestroy()
    {
        foreach (var eventObj in registeredEvents)
        {
            // 清理事件注册
            if (eventObj != null)
            {
                // 具体清理逻辑
            }
        }
        registeredEvents.Clear();

    }

    public virtual async Task OnResumeAsync()
    {
        gameObject.SetActive(true);
        // 子类重写以实现恢复时的逻辑
        await Task.CompletedTask;
    }

    public virtual void SetParameters(object parameters)
    {
        // 基类提供默认实现
    }

    // 关闭自身的方法
    public void Close()
    {
        UIManager.Instance.CloseUI(UIPath);
    }

    protected virtual void TriggerOpenComplete()
    {
        OnOpenComplete?.Invoke();
    }
    
    protected virtual void TriggerCloseComplete()
    {
        OnCloseComplete?.Invoke();
    }
}

// 带参数的UI基类
public abstract class UIBase<T> : UIBase
{
    public override void SetParameters(object parameters)
    {
        if (parameters is T typedParameters)
        {
            OnSetParameters(typedParameters);
        }
        else
        {
            Debug.LogError($"Invalid parameters type. Expected {typeof(T).Name} but got {parameters?.GetType().Name ?? "null"}");
        }
    }

    protected abstract void OnSetParameters(T parameters);
} 