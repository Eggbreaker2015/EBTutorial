using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Toggle))]
public class OneTimeToggleEventInsertor : MonoBehaviour, IPointerClickHandler
{
    private Toggle toggle;

    // 自定义事件，用于在Toggle的onValueChanged之前调用
    public UnityEvent BeforeOnValueChanged = new UnityEvent();
    public Toggle.ToggleEvent OnValueChanged;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        OnValueChanged = toggle.onValueChanged;
        toggle.onValueChanged = new Toggle.ToggleEvent();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 先执行 BeforeOnValueChanged 事件
        BeforeOnValueChanged?.Invoke();
        
        // 恢复并执行 Toggle 的 OnValueChanged 事件
        toggle.onValueChanged = OnValueChanged;
        toggle.OnPointerClick(eventData);

        // 销毁自身
        Destroy(this);
    }
} 