using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace EBTutorial
{
    /// <summary>
    /// 教程系统的UI画布控制器
    /// </summary>
    public class TutorialCanvas : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button maskButton;
        [SerializeField] private Transform arrowNode;

        private Transform arrowNodeParent;
        private TutorialHighlightHandler highlightHandler;
        private TutorialEventHandler eventHandler;
        private Canvas canvas;
        
        public Action OnTutorialStepComplete;

        private TutorialData currTutorialData;
        private TutorialData.StepData currStepData;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializeComponents();
            SetupCanvas();
        }

        #region Public Methods

        /// <summary>
        /// 显示教程步骤
        /// </summary>
        /// <param name="stepData">步骤数据</param>
        /// <param name="viewObject">目标视图对象</param>
        public void ShowTutorial(TutorialData tutorialData, TutorialData.StepData stepData, GameObject viewObject)
        {
            if (stepData == null || viewObject == null)
            {
                Debug.LogError("[TutorialCanvas] Invalid parameters for ShowTutorial");
                return;
            }

            currTutorialData = tutorialData;
            currStepData = stepData;

            Show();
            SetupTutorialStep(tutorialData, stepData, viewObject);
        }

        /// <summary>
        /// 隐藏当前教程
        /// </summary>
        public void HideTutorial()
        {
            CleanupCurrentTutorial();
            Hide();
        }

        #endregion

        #region Private Methods

        private void InitializeComponents()
        {
            arrowNodeParent = arrowNode.parent;
            highlightHandler = new TutorialHighlightHandler();
            eventHandler = new TutorialEventHandler();
            canvas = GetComponent<Canvas>();
        }

        private void SetupCanvas()
        {
            gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
            canvas.sortingOrder = TutorialConst.CanvasSortingOrder;
        }

        private void SetupTutorialStep(TutorialData tutorialData, TutorialData.StepData stepData, GameObject viewObject)
        {
            // 设置高亮
            highlightHandler.SetHighlight(tutorialData, stepData, viewObject);
            
            // 绑定事件
            eventHandler.BindEvents(stepData, viewObject, OnStepFinished);
            
            // 设置箭头位置
            UpdateArrowPosition();
            
            // 处理遮罩
            HandleMask(tutorialData.IsForce);
        }

        private void UpdateArrowPosition()
        {
            var highlightObject = highlightHandler.GetHighlightObject();
            if (highlightObject != null)
            {
                SetArrowPosition(highlightObject.transform);
            }
        }

        private void CleanupCurrentTutorial()
        {
            highlightHandler.ClearHighlight();
            eventHandler.DisableClick();
            ResetArrowPosition();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            HideMask();
            gameObject.SetActive(false);
        }
        
        private void HandleMask(bool isForce)
        {
            if (isForce)
            {
                ShowMask();
            }
            else
            {
                HideMask();
            }
        }

        private void ShowMask(bool fadeIn = true)
        {
            SetupMaskButton(true);
            if (fadeIn)
            {
                //maskButton.image.DOFade(0f, 0.2f).From();
            }
        }
        
        private void HideMask()
        {
            SetupMaskButton(false);
        }

        private void SetupMaskButton(bool isEnabled)
        {
            maskButton.image.enabled = isEnabled;
            
            if (isEnabled && !currStepData.isClickBtn)
            {
                // 延迟按钮点击生效,防止过于快速点击过引导
                StartCoroutine(DelayedEnableButtonClick());
            }
            else
            {
                maskButton.interactable = false;
                maskButton.onClick.RemoveListener(OnStepFinished);
            }
        }

        private IEnumerator DelayedEnableButtonClick()
        {
            yield return new WaitForSeconds(0.3f);
            maskButton.interactable = true;
            maskButton.onClick.AddListener(OnStepFinished);
        }
        
        private void SetArrowPosition(Transform target)
        {
            arrowNode.SetParent(target);

            //TODO: 箭头位置
            arrowNode.localPosition = Vector3.up * 200f;
        }

        private void ResetArrowPosition()
        {
            arrowNode.SetParent(arrowNodeParent);
        }
        
        private void OnStepFinished()
        {
            HideTutorial();
            OnTutorialStepComplete?.Invoke();
        }

        #endregion
    }
}

    


