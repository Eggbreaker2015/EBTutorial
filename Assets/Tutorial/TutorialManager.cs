using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EBTutorial
{
    public class TutorialManager 
    {
        public static TutorialManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TutorialManager();
                }
                return instance;
            }
        }
        private static TutorialManager instance;
        private List<TutorialData> unfinishedTutorialDatas;
        
        private TutorialData currTutorialData;
        private TutorialData.StepData currStepData;
        
        ITutorialStorage tutorialStorage;
        ITutorialCondition tutorialCondition;
        private bool isGuiding;
        private GameObject curViewObject;
        
        private TutorialCanvas tutorialCanvas;

        private Dictionary<int, TutorialProgress> tutorialProgress = new();
        
        public void Initialize(List<TutorialData> tutorialDatas, ITutorialStorage storage, ITutorialCondition condition, TutorialCanvas canvas)
        {
            if (tutorialDatas == null || storage == null || condition == null || canvas == null)
            {
                Debug.LogError("TutorialManager InitData: Invalid parameters");
                return;
            }

            tutorialCanvas = canvas;
            tutorialCanvas.OnTutorialStepComplete += HandleStepComplete;
            tutorialStorage = storage;
            tutorialCondition = condition;
            unfinishedTutorialDatas = new List<TutorialData>(tutorialDatas);

            // 恢复上次的进度
            RestoreTutorialProgress(tutorialDatas);
        }
        
        public void OnOpenUIView(string uiViewName, GameObject viewObject)
        {
            // 如果没有未完成的教程，则提前返回。
            if (unfinishedTutorialDatas == null || !unfinishedTutorialDatas.Any())
                return;

            // 设置当前的视图对象。
            curViewObject = viewObject;

            if (!isGuiding)
            {
                // 查找第一个其初始步骤与打开的视图匹配的教程。
                currTutorialData = unfinishedTutorialDatas
                    .FirstOrDefault(tutorial => tutorial.Steps.FirstOrDefault()?.ViewName == uiViewName);

                // 如果未找到匹配的教程，则提前返回。
                if (currTutorialData == null)
                    return;

                // 获取找到的教程的第一个步骤。
                currStepData = currTutorialData.Steps.First();

                // 如果当前步骤的视图名称与打开的视图不匹配，则提前返回。
                if (currStepData.ViewName != uiViewName)
                    return;

                // 对于第一个步骤，验证附加条件是否满足。
                if (tutorialCondition != null)
                {
                    if (!tutorialCondition.CheckCondition(currTutorialData.Conditions))
                        return;
                }

                // 标记引导过程已开始。
                isGuiding = true;
            }
            else
            {
                // 如果已经在引导中，确保当前步骤与打开的视图匹配。
                if (currStepData.ViewName != uiViewName)
                    return;
            }

            // 在指定的视图对象上显示当前步骤的教程。
            tutorialCanvas.ShowTutorial(currTutorialData, currStepData, viewObject);
        }
        
        public void OnCloseUIView(string uiViewName)
        {
            if (unfinishedTutorialDatas == null || unfinishedTutorialDatas.Count == 0)
                return;
            
            if (currTutorialData.IsForce)
                return;
            
            if (currStepData.ViewName == uiViewName)
            {
                tutorialCanvas.HideTutorial();
            }
        }

        public void ToNextStep(TutorialData.StepData stepData)
        {
            // 保存当前步骤的完成状态
            if (!tutorialProgress.TryGetValue(currTutorialData.TutorialId, out TutorialProgress progress))
            {
                progress = new TutorialProgress
                {
                    TutorialId = currTutorialData.TutorialId,
                    CurrentStep = stepData.Step,
                    IsCompleted = false
                };
                tutorialProgress.Add(currTutorialData.TutorialId, progress);
            }
            
            var nextStep = currTutorialData.Steps.Find(x => x.Step == stepData.Step + 1);
            if (nextStep == null)
            {
                // 教程完成，更新状态
                isGuiding = false;
                progress.Complete();
                SaveTutorialProgress();
                unfinishedTutorialDatas.Remove(currTutorialData);
                tutorialCanvas.HideTutorial();
                return;
            }

            progress.UpdateProgress(nextStep.Step + 1);
            currStepData = nextStep;
            if (nextStep.ViewName == stepData.ViewName)
            {
                tutorialCanvas.ShowTutorial(currTutorialData, currStepData, curViewObject);
            }
            else
            {
                // 如果下一步是在其他界面，先隐藏当前教程
                tutorialCanvas.HideTutorial();
            }
            
            // 保存进度
            SaveTutorialProgress();
        }
        
        private void SaveTutorialProgress()
        {           
            // 可以将进度保存到本地或服务器
            SaveProgressToStorage(tutorialProgress.Values.ToList());
        }
        
        private void RestoreTutorialProgress(List<TutorialData> tutorialDatas)
        {
            List<TutorialProgress> progressList = LoadProgressFromStorage();

            foreach (var tutorialData in tutorialDatas)
            {
                var progress = progressList.FirstOrDefault(p => p.TutorialId == tutorialData.TutorialId);
                if (progress != null && progress.IsCompleted)
                {
                    unfinishedTutorialDatas.Remove(tutorialData);
                }
            }
        }
        
        private void SaveProgressToStorage(List<TutorialProgress> progressList)
        {
            tutorialStorage.SaveProgress(progressList);
        }
        
        private List<TutorialProgress> LoadProgressFromStorage()
        {
            return tutorialStorage.LoadProgress();
        }
        
        public TutorialData.StepData GetCurrentStepData()
        {
            return currStepData;
        }

        private void HandleStepComplete()
        {
            var currentStepData = GetCurrentStepData();
            ToNextStep(currentStepData);
        }
    }

}
