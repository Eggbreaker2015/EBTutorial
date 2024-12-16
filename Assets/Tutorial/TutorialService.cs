using System;
using System.Collections;
using System.Collections.Generic;
using EBTutorial;
using UnityEngine;


//项目相关业务层
public class TutorialService : ITutorialStorage, ITutorialCondition
{
    public bool CheckCondition(TutorialData.Condition[] conditions)
    {
        //TODO: 检查条件
        //这里是和项目当前业务条件比较，比如当前等级，当前金币数量，当前任务状态等
        //举例：
        //if(conditions[0].Key == "Level" && conditions[0].Value == 10)
        //{
        //    return PlayerInfo.Instance.Level >= 10;
        //}
        return true;
    }

    public void SaveProgress(List<TutorialProgress> progressList)
    {
        //存到本地或服务器
    }

    // 从存储中读取所有教程进度
    public List<TutorialProgress> LoadProgress()
    {
        //从本地或服务器读取
        var progressList = new List<TutorialProgress>();
        return progressList;
    }

    public void ClearProgress()
    {
        throw new NotImplementedException();
    }
}


