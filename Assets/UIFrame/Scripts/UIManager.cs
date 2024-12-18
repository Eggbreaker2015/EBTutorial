using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    private Dictionary<string, UIBase> uiCache = new Dictionary<string, UIBase>();
    private Transform uiRoot;
    private Canvas mainCanvas;
    
    // UI栈，用于管理UI打开关闭顺序
    private Stack<UIBase> uiStack = new Stack<UIBase>();

    // 弹窗遮罩预制体引用
    [SerializeField] private GameObject maskPrefab;
    [SerializeField] private List<Transform> layerTransforms;
    
    
    private GameObject maskInstance;

    // 添加配置
    [SerializeField] private int maxCacheCount = 10;
    private Queue<string> cacheQueue = new Queue<string>();
    
    [SerializeField] private UIConfig uiConfig;

    public Action<UIBase> OnUIOpen;
    public Action<UIBase> OnUIClose;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitUIRoot();
            uiConfig.Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitUIRoot()
    {
        // 创建UI根节点
        GameObject root = this.gameObject;
        uiRoot = root.transform;
        DontDestroyOnLoad(root);

        // 创建主Canvas
        mainCanvas = GetComponent<Canvas>();
        maskInstance = Instantiate(maskPrefab, uiRoot);
    }

    private string GetUIPath<T>() where T : UIBase
    {
        string typeName = typeof(T).Name;
        var uiInfo = uiConfig.GetUIInfo(typeName);
        
        if (uiInfo != null)
        {
            return uiInfo.PrefabPath;
        }
        
        // 如果配置中没有找到，使用默认路径规则
        return $"UI/{typeName}.prefab";
    }

    private T OpenUIBase<T>(object parameters, bool isAsync) where T : UIBase
    {
        string uiPath = GetUIPath<T>();

        // 如果是普通界面，关闭当前界面
        if (uiStack.Count > 0)
        {
            var currentUI = uiStack.Peek();
            currentUI.gameObject.SetActive(false);
        }

        // 检查缓存
        if (uiCache.TryGetValue(uiPath, out UIBase cachedUI))
        {
            var typedUI = cachedUI as T;
            if (typedUI != null)
            {
                typedUI.gameObject.SetActive(true);
                typedUI.SetParameters(parameters);
                return typedUI;
            }
        }

        return null;
    }

    private void SetupUI<T>(T ui, string uiPath) where T : UIBase
    {
        if (ui.UILayer == UILayer.Default)
            ui.UILayer = GetDefaultLayer(ui.UIType);
        ui.transform.SetParent(GetLayerTransform(ui.UILayer), false);
        ui.UIPath = uiPath;
        uiCache[uiPath] = ui;
        
        ui.transform.SetAsLastSibling();
        if (ui.UIType == UIType.Popup)
        {
            ShowPopupMask(ui);
        }

        if (ui.UIType == UIType.Normal)
        {
            uiStack.Push(ui);
        }
    }

    public T OpenUI<T>(object parameters = null) where T : UIBase
    {
        var ui = OpenUIBase<T>(parameters, false);
        if (ui != null)
        {
            //ui.OnOpen();
            OnUIOpen?.Invoke(ui);
            return ui;
        }

        string uiPath = GetUIPath<T>();
        var prefab = ResourceManager.Instance.LoadAsset<GameObject>(uiPath);
        if (prefab == null) return null;

        var uiObject = Instantiate(prefab);
        ui = uiObject.GetComponent<T>();
        if (ui == null)
        {
            Debug.LogError($"UI component not found on prefab: {uiPath}");
            return null;
        }

        SetupUI(ui, uiPath);
        ui.SetParameters(parameters);
        //ui.OnOpen();
        OnUIOpen?.Invoke(ui);
        return ui;
    }

    public async Task<T> OpenUIAsync<T>(object parameters = null) where T : UIBase
    {
        var ui = OpenUIBase<T>(parameters, true);
        if (ui != null)
        {
            await ui.OnOpenAsync();
            OnUIOpen?.Invoke(ui);
            return ui;
        }

        string uiPath = GetUIPath<T>();
        var prefab = await ResourceManager.Instance.LoadAssetAsync<GameObject>(uiPath);
        if (prefab == null) return null;

        var uiObject = Instantiate(prefab);
        ui = uiObject.GetComponent<T>();
        if (ui == null)
        {
            Debug.LogError($"UI component not found on prefab: {uiPath}");
            return null;
        }

        SetupUI(ui, uiPath);
        ui.SetParameters(parameters);
        await ui.OnOpenAsync();
        OnUIOpen?.Invoke(ui);
        return ui;
    }

    private void ShowPopupMask(UIBase currentUI)
    {
        if (maskInstance != null)
        {
            maskInstance.SetActive(true);
            maskInstance.transform.SetParent(GetLayerTransform(currentUI.UILayer));
            
            int uiSiblingIndex = currentUI.transform.GetSiblingIndex();
            maskInstance.transform.SetSiblingIndex(uiSiblingIndex);
        }
    }

    public async void CloseUI(string uiPath)
    {
        if (uiCache.TryGetValue(uiPath, out UIBase ui))
        {
            await CloseUIInternal(ui);
            OnUIClose?.Invoke(ui);
        }
    }

    private async Task CloseUIInternal(UIBase ui)
    {
        // 如果是弹窗，移除遮罩
        if (ui.UIType == UIType.Popup)
        {
            maskInstance.SetActive(false);
        }

        // 如果是普通界面，从栈中移除
        if (ui.UIType == UIType.Normal)
        {
            if (uiStack.Count > 0 && uiStack.Peek() == ui)
            {
                uiStack.Pop();
                // 恢复上一个界面
                if (uiStack.Count > 0)
                {
                    var previousUI = uiStack.Peek();
                    previousUI.gameObject.SetActive(true);
                    await previousUI.OnResumeAsync();
                }
            }
        }

        ui.OnClose();
        ResourceManager.Instance.ReleaseAsset(ui.UIPath);
        uiCache.Remove(ui.UIPath);
        Destroy(ui.gameObject);
    }

    private Transform GetLayerTransform(UILayer layer)
    {
        switch (layer)
        {
            case UILayer.Background:
                return layerTransforms[0];
            case UILayer.Normal:
                return layerTransforms[1];
            case UILayer.PopUp:
                return layerTransforms[2];
            case UILayer.Top:
                return layerTransforms[3];
            default:
                throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
        }
    }
    
    private UILayer GetDefaultLayer(UIType type)
    {
        switch (type)
        {
            case UIType.Normal:
                return UILayer.Normal;
            case UIType.Popup:
                return UILayer.PopUp;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    // 预加载API
    public async Task PreloadUIAsync<T>() where T : UIBase
    {
        string uiPath = GetUIPath<T>();
        if (!uiCache.ContainsKey(uiPath))
        {
            var prefab = await ResourceManager.Instance.LoadAssetAsync<GameObject>(uiPath);
            var uiObject = Instantiate(prefab);
            var ui = uiObject.GetComponent<T>();
            uiObject.SetActive(false);
            uiCache[uiPath] = ui;
            ManageCache(uiPath);
        }
    }
    
    private void ManageCache(string uiPath)
    {
        cacheQueue.Enqueue(uiPath);
        if (cacheQueue.Count > maxCacheCount)
        {
            string oldestUI = cacheQueue.Dequeue();
            if (uiCache.TryGetValue(oldestUI, out UIBase ui))
            {
                ResourceManager.Instance.ReleaseAsset(oldestUI);
                Destroy(ui.gameObject);
                uiCache.Remove(oldestUI);
            }
        }
    }
}

public enum UILayer
{
    Default,
    Background,
    Normal,
    PopUp,
    Top
}

public enum UIType
{
    Normal,  // 普通界面，会影响UI栈
    Popup    // 弹窗界面，不影响UI栈
} 