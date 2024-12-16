using System;
using UnityEngine;
using UnityEngine.UI;

namespace EBTutorial
{
    public class TutorialEventHandler
    {
        private Action onStepFinished;
        private bool canClick;
        
        public void BindEvents(TutorialData.StepData stepData, GameObject viewObject, Action onFinished)
        {
            if (!stepData.isClickBtn)
                return;
                
            onStepFinished = onFinished;
            
            var highlightObj = viewObject.transform.Find(stepData.highLightPath).gameObject;
            BindClickEvent(highlightObj);
            EnableClick();
        }

        private void BindClickEvent(GameObject highlightObj)
        {
            // 尝试获取按钮
            if (TryBindButtonEvent(highlightObj))
                return;
                
            // 如果不是按钮，尝试获取Toggle
            TryBindToggleEvent(highlightObj);
        }

        private bool TryBindButtonEvent(GameObject obj)
        {
            var button = obj.GetComponentInChildren<Button>(true);
            if (button == null) 
                return false;
                
            button.gameObject.AddComponent<OneTimeButtonEventInsertor>()
                .BeforeOnClick.AddListener(OnClick);
            return true;
        }

        private bool TryBindToggleEvent(GameObject obj)
        {
            var toggle = obj.GetComponentInChildren<Toggle>(true);
            if (toggle == null) 
                return false;
                
            toggle.gameObject.AddComponent<OneTimeToggleEventInsertor>()
                .BeforeOnValueChanged.AddListener(OnClick);
            return true;
        }
        
        public void EnableClick() => canClick = true;
        
        public void DisableClick() => canClick = false;
        
        private void OnClick()
        {
            if (!canClick) return;
            HandleStepFinished();
        }
        
        private void HandleStepFinished()
        {
            DisableClick();
            onStepFinished?.Invoke();
        }
    }
} 