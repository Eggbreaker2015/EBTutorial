using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ResourcesLoader : IResourceLoader
{
    private Dictionary<string, Object> assetCache = new Dictionary<string, Object>();

    public T LoadAsset<T>(string address) where T : Object
    {
        if (assetCache.ContainsKey(address))
        {
            return assetCache[address] as T;
        }

        T asset = Resources.Load<T>(address);
        if (asset != null)
        {
            assetCache[address] = asset;
            return asset;
        }

        Debug.LogError($"Failed to load asset: {address}");
        return null;
    }

    public async Task<T> LoadAssetAsync<T>(string address) where T : Object
    {
        if (assetCache.ContainsKey(address))
        {
            return assetCache[address] as T;
        }

        try
        {
            var request = Resources.LoadAsync<T>(address);
            await Task.Yield();
            while (!request.isDone)
            {
                await Task.Yield();
            }

            T asset = request.asset as T;
            if (asset != null)
            {
                assetCache[address] = asset;
                return asset;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load asset: {address}, Error: {e.Message}");
        }

        return null;
    }

    public void ReleaseAsset(string address)
    {
        if (assetCache.ContainsKey(address))
        {
            assetCache.Remove(address);
        }
    }

    public void ReleaseAllAssets()
    {
        assetCache.Clear();
        Resources.UnloadUnusedAssets();
    }
}
