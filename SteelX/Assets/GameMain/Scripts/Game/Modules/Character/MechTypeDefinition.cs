using UnityEngine;

[CreateAssetMenu(fileName = "MechTypeDefinition", menuName = "SteelX/Mech/TypeDefinition")]
public class MechTypeDefinition : ScriptableObject
{
    public WeakAssetReference prefabServer;
    public WeakAssetReference prefabClient;
}