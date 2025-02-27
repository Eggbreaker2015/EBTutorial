
using EBTutorial;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public TutorialSO tutorialSO;
    public TutorialCanvas tutorialCanvas;
    // Start is called before the first frame update
    async void Start()
    {

        var tutorialService = new TutorialService();
        TutorialManager.Instance.Initialize(tutorialSO.TutorialDatas, tutorialService, tutorialService, tutorialCanvas);
        // 打开开始菜单
        UIManager.Instance.OnUIOpen += OnUIOpen;
        UIManager.Instance.OnUIClose += OnUIClose;
        await UIManager.Instance.OpenUIAsync<GameStartMenu>();
        
        //StartCombat();
        //TestLoadAsset();
    }

    private void OnUIOpen(UIBase @base)
    {
        TutorialManager.Instance.OnOpenUIView(@base.name, @base.gameObject);
    }

    private void OnUIClose(UIBase @base)
    {
        TutorialManager.Instance.OnCloseUIView(@base.name);
    }
}
