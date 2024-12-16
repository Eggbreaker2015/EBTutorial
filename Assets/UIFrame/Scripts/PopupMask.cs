using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PopupMask : MonoBehaviour
{
    private void Awake()
    {
        var image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.5f); // 半透明黑色
        image.raycastTarget = true; // 确保可以阻挡点击
    }
} 