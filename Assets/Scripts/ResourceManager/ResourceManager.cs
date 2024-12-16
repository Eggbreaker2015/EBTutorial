using UnityEngine;
using System.Threading.Tasks;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ResourceManager");
                instance = go.AddComponent<ResourceManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private IResourceLoader resourceLoader;

    private void Awake()
    {
        #if UNITY_ADDRESSABLES
        resourceLoader = new AddressablesResourceLoader();
        #else
        resourceLoader = new ResourcesLoader();
        #endif
    }

    public T LoadAsset<T>(string address) where T : Object
    {
        return resourceLoader.LoadAsset<T>(address);
    }

    public Task<T> LoadAssetAsync<T>(string address) where T : Object
    {
        return resourceLoader.LoadAssetAsync<T>(address);
    }

    public void ReleaseAsset(string address)
    {
        resourceLoader.ReleaseAsset(address);
    }

    public void ReleaseAllAssets()
    {
        resourceLoader.ReleaseAllAssets();
    }

    private void OnDestroy()
    {
        ReleaseAllAssets();
    }
} 