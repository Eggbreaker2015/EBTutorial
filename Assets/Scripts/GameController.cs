
using EBTutorial;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public TutorialSO tutorialSO;
    public TutorialCanvas tutorialCanvas;
    // Start is called before the first frame update
    void Start()
    {

        var tutorialService = new TutorialService();
        TutorialManager.Instance.Initialize(tutorialSO.TutorialDatas, tutorialService, tutorialService, tutorialCanvas);
        // 打开开始菜单
        UIManager.Instance.OnUIOpen += OnUIOpen;
        UIManager.Instance.OnUIClose += OnUIClose;
        UIManager.Instance.OpenUIAsync<GameStartMenu>();
        
        //StartCombat();
        //TestLoadAsset();
    }

    private async void TestLoadAsset()
    {
        var asset = await ResourceManager.Instance.LoadAssetAsyncTest<GameObject>("GameStartMenu");
        Debug.Log(asset);
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
