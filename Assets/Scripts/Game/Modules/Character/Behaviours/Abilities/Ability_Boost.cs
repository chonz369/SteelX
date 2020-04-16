using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability_Boost", menuName = "SteelX/Abilities/Ability_Boost")]
public class Ability_Boost : CharBehaviorFactory
{
    [Serializable]
    public struct Settings : IComponentData
    {
        public float stopDelay;
    }

    public struct PredictedState : IPredictedComponent<PredictedState>, IComponentData
    {
        public int active;
        public int terminating;
        public int terminateStartTick;

        public static IPredictedComponentSerializerFactory CreateSerializerFactory() {
            return new PredictedComponentSerializerFactory<PredictedState>();
        }

        public void Serialize(ref SerializeContext context, ref NetworkWriter writer) {
            writer.WriteBoolean("active", active == 1);
            writer.WriteBoolean("terminating", terminating == 1);
            writer.WriteInt32("terminateStartTick", terminateStartTick);
        }

        public void Deserialize(ref SerializeContext context, ref NetworkReader reader) {
            active = reader.ReadBoolean() ? 1 : 0;
            terminating = reader.ReadBoolean() ? 1 : 0;
            terminateStartTick = reader.ReadInt32();
        }


#if UNITY_EDITOR
        public bool VerifyPrediction(ref PredictedState state) {
            return true;
        }

        public override string ToString() {
            return "Boost.State active:" + active + " terminating:" + terminating;
        }
#endif
    }

    public Settings settings;

    public override Entity Create(EntityManager entityManager, List<Entity> entities) {
        var entity = CreateCharBehavior(entityManager);
        entities.Add(entity);

        // Ability components
        entityManager.AddComponentData(entity, new PredictedState());
        entityManager.AddComponentData(entity, settings);
        return entity;
    }
}


[DisableAutoCreation]
class Boost_RequestActive : BaseComponentDataSystem<CharBehaviour, AbilityControl,
    Ability_Boost.PredictedState, Ability_Boost.Settings>
{
    public Boost_RequestActive(GameWorld world) : base(world) {
        ExtraComponentRequirements = new ComponentType[] { typeof(ServerEntity) };
    }

    protected override void Update(Entity entity, CharBehaviour charAbility, AbilityControl abilityCtrl,
        Ability_Boost.PredictedState predictedState, Ability_Boost.Settings settings) {
        if (abilityCtrl.behaviorState == AbilityControl.State.Active || abilityCtrl.behaviorState == AbilityControl.State.Cooldown)
            return;

        var command = EntityManager.GetComponentData<UserCommandComponentData>(charAbility.character).command;
        var charPredictedState = EntityManager.GetComponentData<CharacterPredictedData>(charAbility.character);

        abilityCtrl.behaviorState = AbilityControl.State.Idle;

        if (charPredictedState.IsOnGround()) {
            charPredictedState.boostingInAirCount = 0;

            if (command.buttons.IsSet(UserCommand.Button.Boost)) {
                abilityCtrl.behaviorState = AbilityControl.State.RequestActive;
            }
        } else if(charPredictedState.releasedJump == 1 && 
            command.buttons.IsSet(UserCommand.Button.Jump) && charPredictedState.boostingInAirCount == 0) {
            abilityCtrl.behaviorState = AbilityControl.State.RequestActive;
        }

        //todo : check energy

        EntityManager.SetComponentData(entity, abilityCtrl);
        EntityManager.SetComponentData(charAbility.character, charPredictedState);
    }
}


[DisableAutoCreation]
class Boost_Update : BaseComponentDataSystem<CharBehaviour, AbilityControl, Ability_Boost.PredictedState, Ability_Boost.Settings>
{
    public Boost_Update(GameWorld world) : base(world) {
        ExtraComponentRequirements = new ComponentType[] { typeof(ServerEntity) };
    }

    protected override void Update(Entity abilityEntity, CharBehaviour charAbility, AbilityControl abilityCtrl,
        Ability_Boost.PredictedState predictedState, Ability_Boost.Settings settings) {
        if (abilityCtrl.active == 0 && predictedState.active == 0) {
            return;
        }

        var charPredictedState = EntityManager.GetComponentData<CharacterPredictedData>(charAbility.character);

        var command = EntityManager.GetComponentData<UserCommandComponentData>(charAbility.character).command;

        var boostAllowed = charPredictedState.IsOnGround() ? 
            (command.buttons.IsSet(UserCommand.Button.Boost) && !command.buttons.IsSet(UserCommand.Button.Jump)) :
            command.buttons.IsSet(UserCommand.Button.Jump);
        //todo : check energy

        var boostRequested = charPredictedState.IsOnGround() ? command.buttons.IsSet(UserCommand.Button.Boost) :
            (command.buttons.IsSet(UserCommand.Button.Jump) && charPredictedState.releasedJump == 1 && charPredictedState.boostingInAirCount == 0);

        if (boostRequested && boostAllowed && predictedState.active == 0) {
            abilityCtrl.behaviorState = AbilityControl.State.Active;
            if(!charPredictedState.IsOnGround())
                charPredictedState.boostingInAirCount = 1;
            predictedState.active = 1;
            predictedState.terminating = 0;
        }

        var startTerminate = !boostAllowed || abilityCtrl.requestDeactivate == 1;
        if (startTerminate && predictedState.active == 1 && predictedState.terminating == 0) {
            predictedState.terminating = 1;
            predictedState.terminateStartTick = m_world.WorldTime.tick;
        }

        if (predictedState.terminating == 1 && m_world.WorldTime.DurationSinceTick(predictedState.terminateStartTick) >
            settings.stopDelay) {
            abilityCtrl.behaviorState = AbilityControl.State.Idle;
            predictedState.active = 0;
        }

        if (abilityCtrl.active == 0) {
            // Behavior was forcefully deactivated
            abilityCtrl.behaviorState = AbilityControl.State.Idle;
            predictedState.active = 0;
        }

        charPredictedState.boosting = abilityCtrl.behaviorState == AbilityControl.State.Active && predictedState.active == 1 && predictedState.terminating == 0 ? 1 : 0;

        EntityManager.SetComponentData(abilityEntity, abilityCtrl);
        EntityManager.SetComponentData(abilityEntity, predictedState);
        EntityManager.SetComponentData(charAbility.character, charPredictedState);
    }
}