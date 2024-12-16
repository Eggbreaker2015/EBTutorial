using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : UIBase
{
    public Button startButton;

    protected override void InitComponents()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    private void OnStartButtonClick()
    {
        UIManager.Instance.OpenUI<TestMenu1View>();
        
    }
}
