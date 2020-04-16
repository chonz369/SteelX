public class CharacterBehaviours
{
    public static void CreateHandleSpawnSystems(GameWorld world, SystemCollection systems, BundledResourceManager resourceManager, bool server) {
        systems.Add(world.GetECSWorld().CreateSystem<HandleCharacterSpawn>(world, resourceManager, server)); // TODO needs to be done first as it creates presentation
        systems.Add(world.GetECSWorld().CreateSystem<HandleAnimStateCtrlSpawn>(world));
    }

    public static void CreateHandleDespawnSystems(GameWorld world, SystemCollection systems) {
        systems.Add(world.GetECSWorld().CreateSystem<HandleCharacterDespawn>(world));  // TODO HandleCharacterDespawn dewpans char presentation and needs to be called before other HandleDespawn. How do we ensure this ?   
        systems.Add(world.GetECSWorld().CreateSystem<HandleAnimStateCtrlDespawn>(world));
    }

    public static void CreateAbilityRequestSystems(GameWorld world, SystemCollection systems) {
        systems.Add(world.GetECSWorld().CreateSystem<Movement_RequestActive>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Boost_RequestActive>(world));

        // Update main abilities
        systems.Add(world.GetECSWorld().CreateSystem<DefaultBehaviourController_Update>(world));
    }

    public static void CreateMovementStartSystems(GameWorld world, SystemCollection systems) {
        systems.Add(world.GetECSWorld().CreateSystem<Boost_Update>(world));
        systems.Add(world.GetECSWorld().CreateSystem<GroundTest>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Movement_Update>(world));
    }

    public static void CreateMovementResolveSystems(GameWorld world, SystemCollection systems) {
        systems.Add(world.GetECSWorld().CreateSystem<HandleMovementQueries>(world));
        systems.Add(world.GetECSWorld().CreateSystem<Movement_HandleCollision>(world));
    }

    public static void CreateAbilityStartSystems(GameWorld world, SystemCollection systems) {
    }
}
