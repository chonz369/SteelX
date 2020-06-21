using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MechType", menuName = "SteelX/Mech/MechType")]
public class MechTypeAsset : ScriptableObject
{
    public CharacterMoveQuery.Settings characterMovementSettings;

    [AssetType(typeof(ReplicatedEntityFactory))]
    public WeakAssetReference abilities;

    public MechTypeDefinition Mech;
}
