using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class OneTimeButtonEventInsertor : MonoBehaviour, IPointerClickHandler
{
    private Button button;

    // 自定义事件，用于在EventTrigger之前调用
    public UnityEvent BeforeOnClick = new UnityEvent();
    public Button.ButtonClickedEvent OnClick;

    private void Awake()
    {
        button = GetComponent<Button>();
        OnClick = button.onClick;
        button.onClick = new Button.ButtonClickedEvent();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 先执行 BeforeOnClick 事件
        BeforeOnClick?.Invoke();
        button.onClick = OnClick;
        button.onClick.Invoke();
        Destroy(this);
    }
} 