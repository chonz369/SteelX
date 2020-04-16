using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModuleSettings", menuName = "SteelX/Player/PlayerSystemSettings")]
public class PlayerModuleSettings : ScriptableObject
{
    public WeakAssetReference playerStatePrefab;
}
