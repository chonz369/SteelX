using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

public struct DespawningEntity : IComponentData
{
}

[InternalBufferCapacity(16)]
public struct EntityGroupChildren : IBufferElementData
{
    public Entity entity;
}

[DisableAutoCreation]
public class DestroyDespawning : ComponentSystem
{
    EntityQuery Group;

    protected override void OnCreate() {
        base.OnCreate();
        Group = GetEntityQuery(typeof(DespawningEntity));
    }

    protected override void OnUpdate() {
        var entityArray = Group.ToEntityArray(Allocator.TempJob);
        for (var i = 0; i < entityArray.Length; i++) {
            PostUpdateCommands.DestroyEntity(entityArray[i]);
        }
        entityArray.Dispose();
    }
}

public class GameWorld
{
    public GameTime WorldTime;

    public int lastServerTick;

    public float frameDuration;
    public double nextTickTime = 0;

    public GameWorld(string name) {
        WorldTime.tickRate = 60;
        nextTickTime = Game.frameTime;

        m_ECSWorld = World.DefaultGameObjectInjectionWorld;

        m_EntityManager = m_ECSWorld.EntityManager;

        m_destroyDespawningSystem = m_ECSWorld.CreateSystem<DestroyDespawning>();

        if (gameobjectHierarchy.IntValue == 1) {
            m_sceneRoot = new GameObject(name);
            GameObject.DontDestroyOnLoad(m_sceneRoot);
        }
    }

    // SceneRoot can be used to organize crated gameobject in scene view. Is null in standalone.
    public GameObject SceneRoot {
        get { return m_sceneRoot; }
    }

    public void RegisterSceneEntities() {
        // Replicated entities are sorted by their netID and numbered accordingly
        var sceneEntities = new List<ReplicatedEntity>(Object.FindObjectsOfType<ReplicatedEntity>());
        sceneEntities.Sort((a, b) => ByteArrayComp.instance.Compare(a.netID, b.netID));
        for (int i = 0; i < sceneEntities.Count; i++) {
            var gameObjectEntity = sceneEntities[i].GetComponent<GameObjectEntity>();

            var replicatedEntityData = gameObjectEntity.EntityManager.GetComponentData<ReplicatedEntityData>(gameObjectEntity.Entity);
            replicatedEntityData.id = i;
            gameObjectEntity.EntityManager.SetComponentData(gameObjectEntity.Entity, replicatedEntityData);
        }
        m_sceneEntities.AddRange(sceneEntities);
    }

    public World GetECSWorld() {
        return m_ECSWorld;
    }

    public EntityManager GetEntityManager() {
        return m_ECSWorld.EntityManager;
    }

    public void Shutdown() {
        GameObject.Destroy(m_sceneRoot);
    }

    public T Spawn<T>(GameObject prefab) where T : Component {
        return Spawn<T>(prefab, Vector3.zero, Quaternion.identity);
    }

    public T Spawn<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Component {
        Entity entity;
        var gameObject = SpawnInternal(prefab, position, rotation, out entity);
        if (gameObject == null)
            return null;

        var result = gameObject.GetComponent<T>();
        if (result == null) {
            GameDebug.Log(string.Format("Spawned entity '{0}' didn't have component '{1}'", prefab, typeof(T).FullName));
            return null;
        }

        return result;
    }

    public GameObject Spawn(string name, params System.Type[] components) {
        var go = new GameObject(name, components);
        RegisterInternal(go, true);
        return go;
    }

    public GameObject SpawnInternal(GameObject prefab, Vector3 position, Quaternion rotation, out Entity entity) {
        Profiler.BeginSample("GameWorld.SpawnInternal");

        var go = Object.Instantiate(prefab, position, rotation);

        entity = RegisterInternal(go, true);

        Profiler.EndSample();

        return go;
    }

    ////////////////////////////////////////////////////////////////////////////////

    public List<ReplicatedEntity> SceneEntities { get { return m_sceneEntities; } }

    public void RequestDespawn(GameObject entity) {
        if (m_DespawnRequests.Contains(entity)) {
            GameDebug.Assert(false, "Trying to request depawn of same gameobject({0}) multiple times", entity.name);
            return;
        }

        var gameObjectEntity = entity.GetComponent<GameObjectEntity>();
        if (gameObjectEntity != null)
            m_EntityManager.AddComponent(gameObjectEntity.Entity, typeof(DespawningEntity));

        m_DespawnRequests.Add(entity);
    }

    public void RequestDespawn(GameObject entity, EntityCommandBuffer commandBuffer) {
        if (m_DespawnRequests.Contains(entity)) {
            GameDebug.Assert(false, "Trying to request depawn of same gameobject({0}) multiple times", entity.name);
            return;
        }

        var gameObjectEntity = entity.GetComponent<GameObjectEntity>();
        if (gameObjectEntity != null)
            commandBuffer.AddComponent(gameObjectEntity.Entity, new DespawningEntity());

        m_DespawnRequests.Add(entity);
    }

    public void RequestDespawn(Entity entity) {
        m_EntityManager.AddComponent(entity, typeof(DespawningEntity));
        m_DespawnEntityRequests.Add(entity);

        if (m_EntityManager.HasComponent<EntityGroupChildren>(entity)) {
            // Copy buffer as we dont have EntityCommandBuffer to perform changes            
            var buffer = m_EntityManager.GetBuffer<EntityGroupChildren>(entity);
            var entities = new Entity[buffer.Length];
            for (int i = 0; i < buffer.Length; i++) {
                entities[i] = buffer[i].entity;
            }

            for (int i = 0; i < entities.Length; i++) {
                m_EntityManager.AddComponent(entities[i], typeof(DespawningEntity));
                m_DespawnEntityRequests.Add(entities[i]);
            }
        }
    }

    public void RequestDespawn(EntityCommandBuffer commandBuffer, Entity entity) {
        if (m_DespawnEntityRequests.Contains(entity)) {
            GameDebug.Assert(false, "Trying to request depawn of same gameobject({0}) multiple times", entity);
            return;
        }
        commandBuffer.AddComponent(entity, new DespawningEntity());
        m_DespawnEntityRequests.Add(entity);

        if (m_EntityManager.HasComponent<EntityGroupChildren>(entity)) {
            var buffer = m_EntityManager.GetBuffer<EntityGroupChildren>(entity);
            for (int i = 0; i < buffer.Length; i++) {
                commandBuffer.AddComponent(buffer[i].entity, new DespawningEntity());
                m_DespawnEntityRequests.Add(buffer[i].entity);
            }
        }
    }

    public void ProcessDespawns() {
        foreach (var gameObject in m_DespawnRequests) {
            m_dynamicEntities.Remove(gameObject);
            Object.Destroy(gameObject);
        }

        foreach (var entity in m_DespawnEntityRequests) {
            m_EntityManager.DestroyEntity(entity);
        }
        m_DespawnEntityRequests.Clear();
        m_DespawnRequests.Clear();

        m_destroyDespawningSystem.Update();
    }

    Entity RegisterInternal(GameObject gameObject, bool isDynamic) {
        // If gameObject has GameObjectEntity it is already registered in entitymanager. If not we register it here  
        var gameObjectEntity = gameObject.GetComponent<GameObjectEntity>();
        if (gameObjectEntity == null)
            GameObjectEntity.AddToEntityManager(m_EntityManager, gameObject);

        if (isDynamic)
            m_dynamicEntities.Add(gameObject);

        return gameObjectEntity != null ? gameObjectEntity.Entity : Entity.Null;
    }

    DestroyDespawning m_destroyDespawningSystem;

    List<GameObject> m_dynamicEntities = new List<GameObject>();
    List<ReplicatedEntity> m_sceneEntities = new List<ReplicatedEntity>();
    List<GameObject> m_DespawnRequests = new List<GameObject>(32);
    List<Entity> m_DespawnEntityRequests = new List<Entity>(32);

    EntityManager m_EntityManager;
    World m_ECSWorld;

    GameObject m_sceneRoot;

    [ConfigVar(Name = "gameobjecthierarchy", Description = "Should gameobject be organized in a gameobject hierarchy", DefaultValue = "0")]
    static ConfigVar gameobjectHierarchy;
}
