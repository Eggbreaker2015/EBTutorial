using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EBTutorial
{
    public enum ComparisonOperator
    {
        Equal,          // ==
        NotEqual,       // !=
        GreaterThan,    // >
        LessThan,       // <
        GreaterThanOrEqual, // >=
        LessThanOrEqual     // <=
    }

    [Serializable]
    public class TutorialData
    {
        public int TutorialId;
        public bool IsForce; //是否是强制引导，开启会有mask出现
        public Condition[] Conditions;
        public List<StepData> Steps;
        
        [Serializable]
        public class StepData
        {
            public string ViewName; //目标UI名称
            public int Step; //步骤
            public string Desc; //描述
            public string highLightPath; //高亮路径
            public string ArrowTarget; //箭头目标
            public int ArrowDir; //箭头方向
            public Vector2 ArrowOffset; //箭头偏移
            public bool isClickBtn; //是否要点击按钮 如果为false则点击任意位置进入下一步
        }
        
        [Serializable]
        public class Condition
        {
            public string Key;
            public float Value;
            public ComparisonOperator Operator;
            
            public bool CheckCondition(float value)
            {
                return Operator switch
                {
                    ComparisonOperator.Equal => Math.Abs(value - Value) < 0.0001f,
                    ComparisonOperator.NotEqual => Math.Abs(value - Value) > 0.0001f,
                    ComparisonOperator.GreaterThan => value > Value,
                    ComparisonOperator.LessThan => value < Value,
                    ComparisonOperator.GreaterThanOrEqual => value >= Value,
                    ComparisonOperator.LessThanOrEqual => value <= Value,
                    _ => false
                };
            }
        }
    }

    [Serializable]
    public class TutorialProgress
    {
        public int TutorialId;         // 教程ID
        public int CurrentStep;      // 当前步骤
        public bool IsCompleted;        // 是否完成
        
        // 可选：添加额外的进度信息
        public Lazy<Dictionary<string, object>> ExtraData = new Lazy<Dictionary<string, object>>(() => new Dictionary<string, object>(), System.Threading.LazyThreadSafetyMode.None); 
        
        public void UpdateProgress(int step)
        {
            CurrentStep = step;
        }
        
        public void Complete()
        {
            IsCompleted = true;
        }
    }
}
