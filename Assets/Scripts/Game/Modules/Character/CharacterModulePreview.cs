
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Profiling;

public class CharacterModulePreview : CharacterModuleShared
{

    public CharacterModulePreview(GameWorld world, BundledResourceManager resourceSystem) : base(world) {
        // Handle spawn requests
        m_HandleCharacterSpawnRequests = m_world.GetECSWorld().CreateSystem<HandleCharacterSpawnRequests>(m_world, resourceSystem, false);
        m_HandleCharacterDepawnRequests = m_world.GetECSWorld().CreateSystem<HandleCharacterDespawnRequests>(m_world);

        // Handle spawning
        CharacterBehaviours.CreateHandleSpawnSystems(m_world, m_HandleSpawnSystems, resourceSystem, false);

        // Handle despawn
        CharacterBehaviours.CreateHandleDespawnSystems(m_world, m_HandleDespawnSystems);

        // Behaviors 
        CharacterBehaviours.CreateAbilityRequestSystems(m_world, m_AbilityRequestUpdateSystems);
        //m_MovementStartSystems.Add(m_world.GetECSWorld().CreateManager<UpdateTeleportation>(m_world));
        CharacterBehaviours.CreateMovementStartSystems(m_world, m_MovementStartSystems);
        CharacterBehaviours.CreateMovementResolveSystems(m_world, m_MovementResolveSystems);
        CharacterBehaviours.CreateAbilityStartSystems(m_world, m_AbilityStartSystems);
        //CharacterBehaviours.CreateAbilityResolveSystems(m_world, m_AbilityResolveSystems);

        m_UpdateCharPresentationState = m_world.GetECSWorld().CreateSystem<UpdateCharPresentationState>(m_world);
        m_ApplyPresentationState = m_world.GetECSWorld().CreateSystem<ApplyPresentationState>(m_world);
        m_characterCameraSystem = m_world.GetECSWorld().CreateSystem<UpdateCharacterCamera>(m_world);

        m_UpdatePresentationRootTransform = m_world.GetECSWorld().CreateSystem<UpdatePresentationRootTransform>(m_world);

        Console.AddCommand("thirdperson", CmdToggleThirdperson, "Toggle third person mode", this.GetHashCode());
    }

    public override void Shutdown() {
        base.Shutdown();

        m_world.GetECSWorld().DestroySystem(m_HandleCharacterSpawnRequests);
        m_world.GetECSWorld().DestroySystem(m_HandleCharacterDepawnRequests);

        m_world.GetECSWorld().DestroySystem(m_UpdateCharPresentationState);
        m_world.GetECSWorld().DestroySystem(m_characterCameraSystem);

        m_world.GetECSWorld().DestroySystem(m_UpdatePresentationRootTransform);
        m_world.GetECSWorld().DestroySystem(m_ApplyPresentationState);

        Console.RemoveCommandsWithTag(GetHashCode());
    }

    public void HandleSpawnRequests() {
        m_HandleCharacterDepawnRequests.Update();
        m_HandleCharacterSpawnRequests.Update();
    }

    public void HandleDamage() {
    }


    public void UpdatePresentation() {
        m_UpdateCharPresentationState.Update();
        m_ApplyPresentationState.Update();
    }

    public void LateUpdate() {
        m_UpdatePresentationRootTransform.Update();
        m_characterCameraSystem.Update();
    }


    public void UpdateUI() {
    }

    void CmdToggleThirdperson(string[] args) {
        m_characterCameraSystem.ToggleFOrceThirdPerson();
    }

    readonly HandleCharacterSpawnRequests m_HandleCharacterSpawnRequests;
    readonly HandleCharacterDespawnRequests m_HandleCharacterDepawnRequests;

    readonly UpdateCharPresentationState m_UpdateCharPresentationState;
    readonly ApplyPresentationState m_ApplyPresentationState;

    readonly UpdateCharacterCamera m_characterCameraSystem;

    readonly UpdatePresentationRootTransform m_UpdatePresentationRootTransform;
}
