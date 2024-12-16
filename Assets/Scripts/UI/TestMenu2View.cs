using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Game.UI
{
    public partial class TestMenu2View : UIBase
    {
        [SerializeField] private Button button;
        [SerializeField] private Image button1;
        [SerializeField] private TextMeshProUGUI textTMP;

        public Button Button => button;
        public Image Button1 => button1;
        public TextMeshProUGUI TextTMP => textTMP;

        private Action OnButtonClickAction;
        public void SetOnButtonClickHandler(Action handler)
        {
            OnButtonClickAction = handler;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnButtonClickAction?.Invoke());
        }

    }
}
