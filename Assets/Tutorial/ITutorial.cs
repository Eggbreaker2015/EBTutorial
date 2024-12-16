using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EBTutorial
{
    public interface ITutorialStorage
    {
        void SaveProgress(List<TutorialProgress> progressList);
        List<TutorialProgress> LoadProgress();
        void ClearProgress();
    }

    public interface ITutorialCondition
    {
        bool CheckCondition(TutorialData.Condition[] conditions);
    }
}