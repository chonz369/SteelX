using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

public struct CharacterSpawnRequest : IComponentData
{
    public MechSettings mechSettings;
    public Vector3 position;
    public Quaternion rotation;
    public Entity playerEntity;

    private CharacterSpawnRequest(MechSettings mechSettings, Vector3 position, Quaternion rotation, Entity playerEntity) {
        this.mechSettings = mechSettings;
        this.position = position;
        this.rotation = rotation;
        this.playerEntity = playerEntity;
    }

    public static void Create(EntityCommandBuffer commandBuffer, MechSettings mechSettings, Vector3 position, Quaternion rotation, Entity playerEntity) {
        var data = new CharacterSpawnRequest(mechSettings, position, rotation, playerEntity);
        commandBuffer.AddComponent(commandBuffer.CreateEntity(), data);
    }
}

public struct CharacterDespawnRequest : IComponentData
{
    public Entity characterEntity;

    public static void Create(GameWorld world, Entity characterEntity) {
        var data = new CharacterDespawnRequest() {
            characterEntity = characterEntity,
        };
        var entity = world.GetEntityManager().CreateEntity(typeof(CharacterDespawnRequest));
        world.GetEntityManager().SetComponentData(entity, data);
        Debug.Log("create using world & entity");
    }

    public static void Create(EntityCommandBuffer commandBuffer, Entity characterEntity) {
        var data = new CharacterDespawnRequest() {
            characterEntity = characterEntity,
        };
        commandBuffer.AddComponent(commandBuffer.CreateEntity(), data);
    }
}

[DisableAutoCreation]
public class HandleCharacterSpawnRequests : BaseComponentSystem
{
    EntityQuery SpawnGroup;
    CharacterModuleSettings m_settings;

    public HandleCharacterSpawnRequests(GameWorld world, BundledResourceManager resourceManager, bool isServer) : base(world) {
        m_ResourceManager = resourceManager;
        m_settings = Resources.Load<CharacterModuleSettings>("CharacterModuleSettings");
    }

    protected override void OnCreate() {
        base.OnCreate();
        SpawnGroup = GetEntityQuery(typeof(CharacterSpawnRequest));
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Resources.UnloadAsset(m_settings);
    }

    protected override void OnUpdate() {
        var requestArray = SpawnGroup.ToComponentDataArray<CharacterSpawnRequest>(Allocator.TempJob);
        if (requestArray.Length == 0)
            return;

        var requestEntityArray = SpawnGroup.ToEntityArray(Allocator.TempJob);

        // Copy requests as spawning will invalidate Group
        var spawnRequests = new CharacterSpawnRequest[requestArray.Length];
        for (var i = 0; i < requestArray.Length; i++) {
            spawnRequests[i] = requestArray[i];
            PostUpdateCommands.DestroyEntity(requestEntityArray[i]);
        }

        for (var i = 0; i < spawnRequests.Length; i++) {
            var request = spawnRequests[i];
            var playerState = EntityManager.GetComponentObject<PlayerState>(request.playerEntity);
            var character = SpawnCharacter(m_world, playerState, request.position, request.rotation, request.mechSettings, m_ResourceManager);
            playerState.controlledEntity = character.gameObject.GetComponent<GameObjectEntity>().Entity;
        }

        requestArray.Dispose();
        requestEntityArray.Dispose();
    }

    public Character SpawnCharacter(GameWorld world, PlayerState owner, Vector3 position, Quaternion rotation,
        MechSettings mechSettings, BundledResourceManager resourceSystem) {
        var mechTypeRegistry = resourceSystem.GetResourceRegistry<MechTypeRegistry>();

        var mechIndex = Mathf.Min(mechSettings.MechType, mechTypeRegistry.entries.Count);
        var mechTypeAsset = mechTypeRegistry.entries[mechIndex];

        var replicatedEntityRegistry = resourceSystem.GetResourceRegistry<ReplicatedEntityRegistry>();
        var charEntity = replicatedEntityRegistry.Create(EntityManager, resourceSystem, m_world, m_settings.character);

        var character = EntityManager.GetComponentObject<Character>(charEntity);
        character.teamId = 0;
        character.TeleportTo(position, rotation);

        var charRepAll = EntityManager.GetComponentData<CharacterReplicatedData>(charEntity);
        charRepAll.MechSettings = mechSettings;

        charRepAll.abilityCollection = resourceSystem.CreateEntity(mechTypeAsset.abilities);
        EntityManager.SetComponentData(charEntity, charRepAll);

        // Set as predicted by owner
        var replicatedEntity = EntityManager.GetComponentData<ReplicatedEntityData>(charEntity);
        replicatedEntity.predictingPlayerId = owner.playerId;
        EntityManager.SetComponentData(charEntity, replicatedEntity);

        var behaviorCtrlRepEntity = EntityManager.GetComponentData<ReplicatedEntityData>(charRepAll.abilityCollection);
        behaviorCtrlRepEntity.predictingPlayerId = owner.playerId;
        EntityManager.SetComponentData(charRepAll.abilityCollection, behaviorCtrlRepEntity);

        return character;
    }

    readonly BundledResourceManager m_ResourceManager;
}


[DisableAutoCreation]
public class HandleCharacterDespawnRequests : BaseComponentSystem
{
    EntityQuery DespawnGroup;

    public HandleCharacterDespawnRequests(GameWorld world) : base(world) { }

    protected override void OnCreate() {
        base.OnCreate();
        DespawnGroup = GetEntityQuery(typeof(CharacterDespawnRequest));
    }

    protected override void OnUpdate() {
        var requestArray = DespawnGroup.ToComponentDataArray<CharacterDespawnRequest>(Allocator.TempJob);
        if (requestArray.Length != 0) {
            Profiler.BeginSample("HandleCharacterDespawnRequests");

            var requestEntityArray = DespawnGroup.ToEntityArray(Allocator.TempJob);

            for (var i = 0; i < requestArray.Length; i++) {
                var request = requestArray[i];

                var character = EntityManager
                    .GetComponentObject<Character>(request.characterEntity);
                GameDebug.Assert(character != null, "Character despawn request entity is not a character");

                GameDebug.Log("Despawning character:" + character.name + " tick:" + m_world.WorldTime.tick);

                m_world.RequestDespawn(character.gameObject, PostUpdateCommands);

                var charRepAll = EntityManager.GetComponentData<CharacterReplicatedData>(request.characterEntity);
                m_world.RequestDespawn(PostUpdateCommands, charRepAll.abilityCollection);

                PostUpdateCommands.DestroyEntity(requestEntityArray[i]);
            }
            requestEntityArray.Dispose();

            Profiler.EndSample();
        }

        requestArray.Dispose();
        
    }
}

public class CharacterModuleServer : CharacterModuleShared
{
    public CharacterModuleServer(GameWorld world, BundledResourceManager resourceSystem) : base(world) {
        // Handle spawn requests
        m_HandleCharacterSpawnRequests = m_world.GetECSWorld().CreateSystem<HandleCharacterSpawnRequests>(m_world, resourceSystem, true);
        m_HandleCharacterDespawnRequests = m_world.GetECSWorld().CreateSystem<HandleCharacterDespawnRequests>(m_world);

        // Handle spawn
        CharacterBehaviours.CreateHandleSpawnSystems(m_world, m_HandleSpawnSystems, resourceSystem, true);

        // Handle despawn
        CharacterBehaviours.CreateHandleDespawnSystems(m_world, m_HandleDespawnSystems);

        // Behavior
        CharacterBehaviours.CreateAbilityRequestSystems(m_world, m_AbilityRequestUpdateSystems);
        //m_MovementStartSystems.Add(m_world.GetECSWorld().CreateManager<UpdateTeleportation>(m_world));
        CharacterBehaviours.CreateMovementStartSystems(m_world, m_MovementStartSystems);
        CharacterBehaviours.CreateMovementResolveSystems(m_world, m_MovementResolveSystems);
        CharacterBehaviours.CreateAbilityStartSystems(m_world, m_AbilityStartSystems);
        //CharacterBehaviours.CreateAbilityResolveSystems(m_world, m_AbilityResolveSystems);

        m_UpdateCharPresentationState = m_world.GetECSWorld().CreateSystem<UpdateCharPresentationState>(m_world);
        m_ApplyPresentationState = m_world.GetECSWorld().CreateSystem<ApplyPresentationState>(m_world);

        m_UpdatePresentationRootTransform = m_world.GetECSWorld().CreateSystem<UpdatePresentationRootTransform>(m_world);
    }

    public override void Shutdown() {
        base.Shutdown();

        m_world.GetECSWorld().DestroySystem(m_HandleCharacterDespawnRequests);
        m_world.GetECSWorld().DestroySystem(m_HandleCharacterSpawnRequests);
        m_world.GetECSWorld().DestroySystem(m_UpdateCharPresentationState);

        m_world.GetECSWorld().DestroySystem(m_UpdatePresentationRootTransform);
        m_world.GetECSWorld().DestroySystem(m_ApplyPresentationState);
    }

    public void HandleSpawnRequests() {
        m_HandleCharacterDespawnRequests.Update();
        m_HandleCharacterSpawnRequests.Update();
    }

    public void PresentationUpdate() {
        m_UpdateCharPresentationState.Update();
        m_ApplyPresentationState.Update();
    }

    public void AttachmentUpdate() {
        m_UpdatePresentationRootTransform.Update();
    }

    public void CleanupPlayer(PlayerState player) {
        if (player.controlledEntity != Entity.Null) {
            CharacterDespawnRequest.Create(m_world, player.controlledEntity);
        }
    }

    readonly HandleCharacterSpawnRequests m_HandleCharacterSpawnRequests;
    readonly HandleCharacterDespawnRequests m_HandleCharacterDespawnRequests;

    readonly UpdateCharPresentationState m_UpdateCharPresentationState;
    readonly ApplyPresentationState m_ApplyPresentationState;

    readonly UpdatePresentationRootTransform m_UpdatePresentationRootTransform;
}
