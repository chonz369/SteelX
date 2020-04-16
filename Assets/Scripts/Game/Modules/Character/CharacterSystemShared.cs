using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Profiling;


[DisableAutoCreation]
public class HandleCharacterSpawn : InitializeComponentGroupSystem<Character, HandleCharacterSpawn.Initialized>
{
    public struct Initialized : IComponentData { }

    List<Character> characters = new List<Character>();
    bool server;
    public HandleCharacterSpawn(GameWorld gameWorld, BundledResourceManager resourceManager, bool server) : base(gameWorld) {
        m_resourceManager = resourceManager;
        this.server = server;
    }

    private List<Entity> entityBuffer = new List<Entity>(8);
    private List<Character> characterBuffer = new List<Character>(8);
    protected override void Initialize(ref EntityQuery group) {
        // We are not allowed to spawn prefabs while iterating ComponentGroup so we copy list of entities and characters.
        entityBuffer.Clear();
        characterBuffer.Clear();
        {
            var entityArray = group.ToEntityArray(Allocator.TempJob);
            var characterArray = group.ToComponentArray<Character>();
            for (var i = 0; i < entityArray.Length; i++) {
                entityBuffer.Add(entityArray[i]);
                characterBuffer.Add(characterArray[i]);
            }
            entityArray.Dispose();
        }

        for (var i = 0; i < entityBuffer.Count; i++) {
            var charEntity = entityBuffer[i];
            var character = characterBuffer[i];

            var characterRepAll = EntityManager.GetComponentData<CharacterReplicatedData>(charEntity);

            MechTypeRegistry mechTypeRegistry = m_resourceManager.GetResourceRegistry<MechTypeRegistry>();
            MechTypeAsset mechTypeAsset = mechTypeRegistry.entries[characterRepAll.MechSettings.MechType];
            character.MechTypeData = mechTypeAsset;

            MechTypeDefinition mechTypeDefinition = mechTypeAsset.Mech;

            //// Create main presentation
            var mechPrefabGUID = server ? mechTypeDefinition.prefabServer : mechTypeDefinition.prefabClient;
            var mechPrefab = m_resourceManager.GetSingleAssetResource(mechPrefabGUID) as GameObject;
            var presentationGOE = m_world.Spawn<GameObjectEntity>(mechPrefab);
            var charPresentationEntity = presentationGOE.Entity;

            //Build mech
            MechPresentationBuilder.Build(m_resourceManager, presentationGOE.GetComponent<MechPresentationSetup>(), 
                presentationGOE.GetComponent<MechSkeleton>(), characterRepAll.MechSettings, server);

            character.presentation = charPresentationEntity;

            var charPresentation = EntityManager.GetComponentObject<MechPresentationSetup>(charPresentationEntity);
            charPresentation.character = charEntity;
            character.presentations.Add(charPresentation);


            // Setup CharacterMoveQuery
            var moveQuery = EntityManager.GetComponentObject<CharacterMoveQuery>(charEntity);
            moveQuery.Initialize(mechTypeAsset.characterMovementSettings, charEntity);

            // Setup abilities
            GameDebug.Assert(EntityManager.Exists(characterRepAll.abilityCollection), "behavior controller entity does not exist");
            var buffer = EntityManager.GetBuffer<EntityGroupChildren>(characterRepAll.abilityCollection);
            for (int j = 0; j < buffer.Length; j++) {
                var childEntity = buffer[j].entity;
                if (EntityManager.HasComponent<CharBehaviour>(childEntity)) {
                    var charBehaviour = EntityManager.GetComponentData<CharBehaviour>(childEntity);
                    charBehaviour.character = charEntity;
                    EntityManager.SetComponentData(childEntity, charBehaviour);
                }
            }
        }
    }


    BundledResourceManager m_resourceManager;

}

[DisableAutoCreation]
public class HandleCharacterDespawn : DeinitializeComponentSystem<Character>
{
    List<Character> characters = new List<Character>();

    public HandleCharacterDespawn(GameWorld gameWorld) : base(gameWorld) { }

    protected override void Deinitialize(Entity entity, Character character) {
        var charEntity = character.GetComponent<GameObjectEntity>().Entity;

        var moveQuery = EntityManager.GetComponentObject<CharacterMoveQuery>(charEntity);
        moveQuery.Shutdown();

        // Remove presentations
        foreach (var charPresentation in character.presentations) {
            m_world.RequestDespawn(charPresentation.gameObject, PostUpdateCommands);
        }
    }
}

[DisableAutoCreation]
public class UpdateCharPresentationState : BaseComponentSystem
{
    EntityQuery Group;
    const float k_StopMovePenalty = 0.075f;

    public UpdateCharPresentationState(GameWorld gameWorld) : base(gameWorld) { }

    protected override void OnCreate() {
        base.OnCreate();
        Group = GetEntityQuery(typeof(ServerEntity), typeof(Character), typeof(CharacterPredictedData), typeof(CharacterInterpolatedData),
            typeof(UserCommandComponentData));
    }


    protected override void OnUpdate() {
        Profiler.BeginSample("CharacterSystemShared.UpdatePresentationState");

        var entityArray = Group.ToEntityArray(Allocator.TempJob);
        var characterArray = Group.ToComponentArray<Character>();
        var charPredictedStateArray = Group.ToComponentDataArray<CharacterPredictedData>(Allocator.TempJob);
        var charAnimStateArray = Group.ToComponentDataArray<CharacterInterpolatedData>(Allocator.TempJob);
        var userCommandArray = Group.ToComponentDataArray<UserCommandComponentData>(Allocator.TempJob);

        var deltaTime = m_world.frameDuration;
        for (var i = 0; i < charPredictedStateArray.Length; i++) {
            var entity = entityArray[i];
            var character = characterArray[i];
            var charPredictedState = charPredictedStateArray[i];
            var animState = charAnimStateArray[i];
            var userCommand = userCommandArray[i].command;

            // TODO: Move this into the network
            animState.position = charPredictedState.position;
            animState.charLocoTick = charPredictedState.locoStartTick;
            animState.sprinting = charPredictedState.boosting;
            animState.charAction = charPredictedState.action;
            animState.charActionTick = charPredictedState.actionStartTick;
            animState.aimYaw = userCommand.lookYaw;
            animState.aimPitch = userCommand.lookPitch;
            animState.previousCharLocoState = animState.charLocoState;

            // Add small buffer between GroundMove and Stand, to reduce animation noise when there are gaps in between
            // input keypresses
            if (charPredictedState.locoState == CharacterPredictedData.LocoState.Stand
                && animState.charLocoState == CharacterPredictedData.LocoState.GroundMove
                && m_world.WorldTime.DurationSinceTick(animState.lastGroundMoveTick) < k_StopMovePenalty) {
                animState.charLocoState = CharacterPredictedData.LocoState.GroundMove;
            } else {
                animState.charLocoState = charPredictedState.locoState;
            }

            var groundMoveVec = Vector3.ProjectOnPlane(charPredictedState.velocity, Vector3.up);
            animState.moveYaw = Vector3.Angle(Vector3.forward, groundMoveVec);
            var cross = Vector3.Cross(Vector3.forward, groundMoveVec);
            if (cross.y < 0)
                animState.moveYaw = 360 - animState.moveYaw;

            animState.damageTick = charPredictedState.damageTick;
            var damageDirOnPlane = Vector3.ProjectOnPlane(charPredictedState.damageDirection, Vector3.up);
            animState.damageDirection = Vector3.SignedAngle(Vector3.forward, damageDirOnPlane, Vector3.up);

            // Set anim state before anim state ctrl is running 
            EntityManager.SetComponentData(entity, animState);

            // TODO perhaps we should not call presentation, but make system that updates presentation (and reads anim state) 
            // Update presentationstate animstatecontroller
            var animStateCtrl = EntityManager.GetComponentObject<AnimStateController>(character.presentation);
            animStateCtrl.UpdatePresentationState(m_world.WorldTime, deltaTime);

            if (charPredictedState.locoState == CharacterPredictedData.LocoState.GroundMove) {
                animState = EntityManager.GetComponentData<CharacterInterpolatedData>(entity);
                animState.lastGroundMoveTick = m_world.WorldTime.tick;
                EntityManager.SetComponentData(entity, animState);
            }
        }

        entityArray.Dispose();
        charPredictedStateArray.Dispose();
        charAnimStateArray.Dispose();
        userCommandArray.Dispose();

        Profiler.EndSample();
    }
}


[DisableAutoCreation]
public class GroundTest : BaseComponentSystem
{
    EntityQuery Group;

    public GroundTest(GameWorld gameWorld) : base(gameWorld) {
        m_defaultLayer = LayerMask.NameToLayer("Default");
        m_playerLayer = LayerMask.NameToLayer("collision_player");
        m_platformLayer = LayerMask.NameToLayer("Platform");

        m_mask = 1 << m_defaultLayer | 1 << m_playerLayer | 1 << m_platformLayer;
    }

    protected override void OnCreate() {
        base.OnCreate();
        Group = GetEntityQuery(typeof(ServerEntity), typeof(Character), typeof(CharacterPredictedData));
    }

    protected override void OnUpdate() {
        var charPredictedStateArray = Group.ToComponentDataArray<CharacterPredictedData>(Allocator.TempJob);
        var characterArray = Group.ToComponentArray<Character>();

        var startOffset = 1f;
        var distance = 3f;

        var rayCommands = new NativeArray<RaycastCommand>(charPredictedStateArray.Length, Allocator.TempJob);
        var rayResults = new NativeArray<RaycastHit>(charPredictedStateArray.Length, Allocator.TempJob);

        for (var i = 0; i < charPredictedStateArray.Length; i++) {
            var charPredictedState = charPredictedStateArray[i];
            var origin = charPredictedState.position + Vector3.up * startOffset;
            rayCommands[i] = new RaycastCommand(origin, Vector3.down, distance, m_mask);
        }

        var handle = RaycastCommand.ScheduleBatch(rayCommands, rayResults, 10);
        handle.Complete();

        for (var i = 0; i < characterArray.Length; i++) {
            var character = characterArray[i];
            character.groundCollider = rayResults[i].collider;
            character.altitude = character.groundCollider != null ? rayResults[i].distance - startOffset : distance - startOffset;

            if (character.groundCollider != null)
                character.groundNormal = rayResults[i].normal;
        }

        rayCommands.Dispose();
        rayResults.Dispose();

        charPredictedStateArray.Dispose();
    }

    readonly int m_defaultLayer;
    readonly int m_playerLayer;
    readonly int m_platformLayer;
    readonly int m_mask;
}

[DisableAutoCreation]
public class ApplyPresentationState : BaseComponentSystem
{
    EntityQuery CharGroup;

    public ApplyPresentationState(GameWorld world) : base(world) { }

    protected override void OnCreate() {
        base.OnCreate();
        CharGroup = GetEntityQuery(typeof(AnimStateController), typeof(MechPresentationSetup), ComponentType.Exclude<DespawningEntity>());
    }

    protected override void OnUpdate() {
        var deltaTime = m_world.frameDuration;
        var animStateCtrlArray = CharGroup.ToComponentArray<AnimStateController>();

        Profiler.BeginSample("CharacterSystemShared.ApplyPresentationState");

        for (var i = 0; i < animStateCtrlArray.Length; i++) {
            var animStateCtrl = animStateCtrlArray[i];
            animStateCtrl.ApplyPresentationState(m_world.WorldTime, deltaTime);
        }

        Profiler.EndSample();
    }

}



