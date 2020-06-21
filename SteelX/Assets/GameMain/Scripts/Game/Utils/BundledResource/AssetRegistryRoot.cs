using UnityEngine;

[CreateAssetMenu(fileName = "AssetRegistryRoot", menuName = "SteelX/Resource/AssetRegistryRoot", order = 10000)]
public class AssetRegistryRoot : ScriptableObject
{
    public bool serverBuild;
    public ScriptableObject[] assetRegistries;
}
