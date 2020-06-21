using UnityEngine;
using System;
using Unity.Entities;
using UnityEngine.Profiling;

[Serializable]
public struct CharacterInterpolatedData : IInterpolatedComponent<CharacterInterpolatedData>, IComponentData
{
    public Vector3 position;
    public float rotation;
    public float aimYaw;
    public float aimPitch;
    public float moveYaw;                                       // Global rotation 0->360 deg

    public CharacterPredictedData.LocoState charLocoState;
    public int charLocoTick;
    public CharacterPredictedData.Action charAction;
    public int charActionTick;
    public int damageTick;
    public float damageDirection;
    public int boosting;

    // Custom properties for Animation states
    public CharacterPredictedData.LocoState previousCharLocoState;
    public int lastGroundMoveTick;
    public float moveAngleLocal;                                // Movement rotation realtive to character forward -180->180 deg clockwise
    public float shootPoseWeight;
    public Vector2 locomotionVector;
    public float locomotionPhase;
    public float landAnticWeight;
    public float turnStartAngle;//todo :remove this
    public short turnDirection; //todo :remove this                                // -1 TurnLeft, 0 Idle, 1 TurnRight
    public float inAirTime;
    public float jumpTime;
    public float simpleTime;
    public Vector2 footIkOffset;
    public Vector3 footIkNormalLeft;
    public Vector3 footIkNormaRight;

    public static IInterpolatedComponentSerializerFactory CreateSerializerFactory() {
        return new InterpolatedComponentSerializerFactory<CharacterInterpolatedData>();
    }

    public void Serialize(ref SerializeContext context, ref NetworkWriter writer) {
        writer.WriteVector3Q("position", position, 2);
        writer.WriteFloatQ("rotation", rotation, 0);
        writer.WriteFloatQ("aimYaw", aimYaw, 0);
        writer.WriteFloatQ("aimPitch", aimPitch, 0);
        writer.WriteFloatQ("moveYaw", moveYaw, 0);

        writer.WriteInt32("charLocoState", (int)charLocoState);
        writer.WriteInt32("charLocoTick", charLocoTick);
        writer.WriteInt32("characterAction", (int)charAction);
        writer.WriteInt32("characterActionTick", charActionTick);
        writer.WriteBoolean("sprinting", boosting == 1);
        writer.WriteInt32("damageTick", damageTick);
        writer.WriteFloatQ("damageDirection", damageDirection, 1);

        writer.WriteFloatQ("moveAngleLocal", moveAngleLocal, 0);
        //writer.WriteFloatQ("shootPoseWeight", shootPoseWeight);
        writer.WriteVector2Q("locomotionVector", locomotionVector);
        writer.WriteFloatQ("locomotionPhase", locomotionPhase);
        writer.WriteFloatQ("landAnticWeight", landAnticWeight, 2);
        writer.WriteFloatQ("inAirTime", inAirTime, 2);
        writer.WriteFloatQ("jumpTime", jumpTime, 2);
        //writer.WriteFloatQ("simpleTime", simpleTime, 2);
        //writer.WriteVector2Q("footIkOffset", footIkOffset, 2);
        //writer.WriteVector3Q("footIkNormalLeft", footIkNormalLeft, 2);
        //writer.WriteVector3Q("footIkNormaRight", footIkNormaRight, 2);
    }

    public void Deserialize(ref SerializeContext context, ref NetworkReader reader) {
        position = reader.ReadVector3Q();
        rotation = reader.ReadFloatQ();
        aimYaw = reader.ReadFloatQ();
        aimPitch = reader.ReadFloatQ();
        moveYaw = reader.ReadFloatQ();

        charLocoState = (CharacterPredictedData.LocoState)reader.ReadInt32();
        charLocoTick = reader.ReadInt32();
        charAction = (CharacterPredictedData.Action)reader.ReadInt32();
        charActionTick = reader.ReadInt32();
        boosting = reader.ReadBoolean() ? 1 : 0;

        damageTick = reader.ReadInt32();
        damageDirection = reader.ReadFloatQ();

        moveAngleLocal = reader.ReadFloatQ();
        //shootPoseWeight = reader.ReadFloatQ();
        locomotionVector = reader.ReadVector2Q();
        locomotionPhase = reader.ReadFloatQ();
        landAnticWeight = reader.ReadFloatQ();
        inAirTime = reader.ReadFloatQ();
        jumpTime = reader.ReadFloatQ();
        //simpleTime = reader.ReadFloatQ();
        //footIkOffset = reader.ReadVector2Q();
        //footIkNormalLeft = reader.ReadVector3Q();
        //footIkNormaRight = reader.ReadVector3Q();
    }

    public void Interpolate(ref SerializeContext context, ref CharacterInterpolatedData prevState,
        ref CharacterInterpolatedData nextState, float f) {
        position = Vector3.Lerp(prevState.position, nextState.position, f);
        rotation = Mathf.LerpAngle(prevState.rotation, nextState.rotation, f);
        aimYaw = Mathf.LerpAngle(prevState.aimYaw, nextState.aimYaw, f);
        aimPitch = Mathf.LerpAngle(prevState.aimPitch, nextState.aimPitch, f);
        moveYaw = Mathf.LerpAngle(prevState.moveYaw, nextState.moveYaw, f);

        charLocoState = prevState.charLocoState;
        charLocoTick = prevState.charLocoTick;
        charAction = prevState.charAction;
        charActionTick = prevState.charActionTick;
        boosting = prevState.boosting;

        damageTick = prevState.damageTick;
        damageDirection = prevState.damageDirection;

        moveAngleLocal = Mathf.LerpAngle(prevState.moveAngleLocal, nextState.moveAngleLocal, f);
        shootPoseWeight = Mathf.Lerp(prevState.shootPoseWeight, nextState.shootPoseWeight, f);
        locomotionVector = Vector2.Lerp(prevState.locomotionVector, nextState.locomotionVector, f);
        locomotionPhase = Mathf.Lerp(prevState.locomotionPhase, nextState.locomotionPhase, f);
        landAnticWeight = Mathf.Lerp(prevState.landAnticWeight, nextState.landAnticWeight, f);
        inAirTime = Mathf.Lerp(prevState.inAirTime, nextState.inAirTime, f);
        jumpTime = Mathf.Lerp(prevState.jumpTime, nextState.jumpTime, f);
        simpleTime = Mathf.Lerp(prevState.simpleTime, nextState.simpleTime, f);
        footIkOffset = Vector2.Lerp(prevState.footIkOffset, nextState.footIkOffset, f);
        footIkNormalLeft = Vector3.Lerp(prevState.footIkNormalLeft, nextState.footIkNormalLeft, f);
        footIkNormaRight = Vector3.Lerp(prevState.footIkNormaRight, nextState.footIkNormaRight, f);
    }

    public override string ToString() {
        System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
        strBuilder.AppendLine("position" + position);
        strBuilder.AppendLine("rotation" + rotation);
        strBuilder.AppendLine("aimYaw" + aimYaw);
        strBuilder.AppendLine("aimPitch" + aimPitch);
        strBuilder.AppendLine("moveYaw" + moveYaw);
        strBuilder.AppendLine("charLocoState" + charLocoState);
        strBuilder.AppendLine("charLocoTick" + charLocoTick);
        strBuilder.AppendLine("characterAction" + charAction);
        strBuilder.AppendLine("characterActionTick" + charActionTick);
        strBuilder.AppendLine("sprinting" + boosting);
        strBuilder.AppendLine("shootPoseWeight" + shootPoseWeight);
        strBuilder.AppendLine("locomotionVector" + locomotionVector);
        strBuilder.AppendLine("locomotionPhase" + locomotionPhase);
        strBuilder.AppendLine("landAnticWeight" + landAnticWeight);
        strBuilder.AppendLine("footIkOffset" + footIkOffset);

        return strBuilder.ToString();
    }
}

public class CharacterInterpolated : ComponentDataProxy<CharacterInterpolatedData>
{ }