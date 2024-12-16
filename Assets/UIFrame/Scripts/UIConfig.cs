using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "EggFrame/UIConfig", order = 0)]
public class UIConfig : ScriptableObject
{
    [System.Serializable]
    public class UIInfo
    {
        public string TypeName;
        public string PrefabPath;
        public UILayer DefaultLayer;
        public int SortingOrder;
    }

    public List<UIInfo> UIInfos = new List<UIInfo>();
    
    private Dictionary<string, UIInfo> uiInfoMap;
    
    public void Initialize()
    {
        uiInfoMap = UIInfos.ToDictionary(info => info.TypeName);
    }
    
    public UIInfo GetUIInfo(string typeName)
    {
        return uiInfoMap.TryGetValue(typeName, out var info) ? info : null;
    }
} 