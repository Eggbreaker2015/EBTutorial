using UnityEngine;
using UnityEngine.UI;

namespace EBTutorial
{
    public class TutorialHighlightHandler
    {
        private GameObject highlightObject;

        
        public void SetHighlight(TutorialData tutorialData, TutorialData.StepData stepData, GameObject viewObject)
        {
            if (string.IsNullOrEmpty(stepData.highLightPath))
                return;
                
            highlightObject = viewObject.transform.Find(stepData.highLightPath).gameObject;
            
            if (tutorialData.IsForce)
            {
                SetForceHighlight(stepData);
            }
        }
        
        private void SetForceHighlight(TutorialData.StepData stepData)
        {
            var inheritedCanvas = highlightObject.AddComponent<Canvas>();
            inheritedCanvas.overrideSorting = true;
            inheritedCanvas.sortingOrder = TutorialConst.HighlightSortingOrder;
            
            if (stepData.isClickBtn)
            {
                highlightObject.AddComponent<GraphicRaycaster>();
            }
        }
        
        public void ClearHighlight()
        {
            if (highlightObject == null)
                return;
                
            GameObject.Destroy(highlightObject.GetComponent<GraphicRaycaster>());
            GameObject.Destroy(highlightObject.GetComponent<Canvas>());
            highlightObject = null;
        }
        
        public GameObject GetHighlightObject()
        {
            return highlightObject;
        }
    }
} 