using System.Collections.Generic;
using UnityEngine;

namespace EBTutorial
{
    [CreateAssetMenu(fileName = "TutorialSO", menuName = "Tutorial/TutorialSO")]
    public class TutorialSO : ScriptableObject
    {
        public List<TutorialData> TutorialDatas;
    }
}