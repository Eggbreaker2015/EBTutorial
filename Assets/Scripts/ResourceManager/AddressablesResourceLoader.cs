#if UNITY_ADDRESSABLES

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System.Collections.Generic;

public class AddressablesResourceLoader : IResourceLoader
{
    private Dictionary<string, Object> assetCache = new Dictionary<string, Object>();
    private Dictionary<string, AsyncOperationHandle> handleCache = new Dictionary<string, AsyncOperationHandle>();

    public T LoadAsset<T>(string address) where T : Object
    {
        if (assetCache.ContainsKey(address))
        {
            return assetCache[address] as T;
        }

        var operation = Addressables.LoadAssetAsync<T>(address);
        operation.WaitForCompletion();

        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            T asset = operation.Result;
            assetCache[address] = asset;
            handleCache[address] = operation;
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
            var operation = Addressables.LoadAssetAsync<T>(address);
            operation.WaitForCompletion();

            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                T asset = operation.Result;
                assetCache[address] = asset;
                handleCache[address] = operation;
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
        if (handleCache.TryGetValue(address, out AsyncOperationHandle handle))
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
            handleCache.Remove(address);
        }

        if (assetCache.ContainsKey(address))
        {
            assetCache.Remove(address);
        }
    }

    public void ReleaseAllAssets()
    {
        foreach (var handle in handleCache.Values)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        handleCache.Clear();
        assetCache.Clear();
    }
}
#endif