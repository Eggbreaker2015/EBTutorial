using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Game.UI
{
    public partial class TestMenu1View : UIBase
    {
        [SerializeField] private Button button;
        [SerializeField] private Image button1;
        [SerializeField] private TextMeshProUGUI textTMP;

        public Button Button => button;
        public Image Button1 => button1;
        public TextMeshProUGUI TextTMP => textTMP;

        private Action OnButtonClickAction;
        
        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            UIManager.Instance.OpenUI<TestMenu2View>();
        }

    }
}
