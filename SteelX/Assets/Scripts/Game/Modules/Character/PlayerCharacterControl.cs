using System;
using UnityEngine;

[Serializable]
public struct MechSettings : IReplicatedComponent
{
    public int MechType;
    public int Head;
    public int Core;
    public int Arms;
    public int Legs;
    public int Booster;
    public int Weapon1L;
    public int Weapon1R;
    public int Weapon2L;
    public int Weapon2R;

    public void Serialize(ref SerializeContext context, ref NetworkWriter writer) {
        //todo : implement this
    }

    public void Deserialize(ref SerializeContext context, ref NetworkReader reader) {
        //todo : implement this
    }
}

public class PlayerCharacterControl : MonoBehaviour
{
    public bool RequestMechChange;
    public MechSettings MechSettings;
    public MechSettings RequestedMechSettings;
}
