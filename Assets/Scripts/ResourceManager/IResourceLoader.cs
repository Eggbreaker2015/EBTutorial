using UnityEngine;
using System.Threading.Tasks;

public interface IResourceLoader
{
    T LoadAsset<T>(string address) where T : Object;
    Task<T> LoadAssetAsync<T>(string address) where T : Object;
    void ReleaseAsset(string address);
    void ReleaseAllAssets();
}
