

using Game;
using UnityEngine;

public class Attachment : MonoBehaviour
{

    private AssetInfo assetInfo = null;
    
    internal static void Create(GameObject obj, AssetInfo assetInfo)
    {
        var behaviour = obj.AddComponent<Attachment>();
        behaviour.assetInfo = assetInfo;
    }

    void OnDestroy()
    {
        if(null != assetInfo)
        {
            Resource.Free(assetInfo);
        }
    }

    
}
