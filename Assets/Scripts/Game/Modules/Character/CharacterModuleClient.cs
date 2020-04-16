class CharacterModuleClient : CharacterModuleShared
{
    public CharacterModuleClient(GameWorld world, BundledResourceManager resourceSystem) : base(world) {
        // Handle spawn
        CharacterBehaviours.CreateHandleSpawnSystems(m_world, m_HandleSpawnSystems, resourceSystem, false);

        // Handle despawn
        CharacterBehaviours.CreateHandleDespawnSystems(m_world, m_HandleDespawnSystems);

        // Behaviors
        CharacterBehaviours.CreateAbilityRequestSystems(m_world, m_AbilityRequestUpdateSystems);
        CharacterBehaviours.CreateMovementStartSystems(m_world, m_MovementStartSystems);
        CharacterBehaviours.CreateMovementResolveSystems(m_world, m_MovementResolveSystems);
        CharacterBehaviours.CreateAbilityStartSystems(m_world, m_AbilityStartSystems);
        //CharacterBehaviours.CreateAbilityResolveSystems(m_world, m_AbilityResolveSystems);

        // Interpolation        
        m_UpdateCharPresentationState = m_world.GetECSWorld().CreateSystem<UpdateCharPresentationState>(m_world);
        m_ApplyPresentationState = m_world.GetECSWorld().CreateSystem<ApplyPresentationState>(m_world);
        m_UpdatePresentationRootTransform = m_world.GetECSWorld().CreateSystem<UpdatePresentationRootTransform>(m_world);

        characterCameraSystem = m_world.GetECSWorld().CreateSystem<UpdateCharacterCamera>(m_world);
    }

    public override void Shutdown() {
        base.Shutdown();

        m_world.GetECSWorld().DestroySystem(m_UpdateCharPresentationState);
        m_world.GetECSWorld().DestroySystem(m_ApplyPresentationState);
        m_world.GetECSWorld().DestroySystem(m_UpdatePresentationRootTransform);

        m_world.GetECSWorld().DestroySystem(characterCameraSystem);
    }

    public void Interpolate() {
    }

    public void UpdatePresentation() {
        m_UpdateCharPresentationState.Update();
        m_ApplyPresentationState.Update();
    }

    public void LateUpdate() {
    }

    public void CameraUpdate() {
        m_UpdatePresentationRootTransform.Update();
        characterCameraSystem.Update();
    }

    readonly UpdateCharPresentationState m_UpdateCharPresentationState;
    readonly ApplyPresentationState m_ApplyPresentationState;

    readonly UpdatePresentationRootTransform m_UpdatePresentationRootTransform;

    readonly UpdateCharacterCamera characterCameraSystem;
}
