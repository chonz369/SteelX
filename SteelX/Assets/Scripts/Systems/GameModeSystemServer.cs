using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Profiling;
using Unity.Collections;

public interface IGameMode
{
    void Initialize(GameWorld world, GameModeSystemServer gameModeSystemServer);
    void Shutdown();

    void Restart();
    void Update();

    void OnPlayerJoin(PlayerState player);
    void OnPlayerRespawn(PlayerState player, ref Vector3 position, ref Quaternion rotation);
    void OnPlayerKilled(PlayerState victim, PlayerState killer);
}

public class NullGameMode : IGameMode
{
    public void Initialize(GameWorld world, GameModeSystemServer gameModeSystemServer) { }
    public void OnPlayerJoin(PlayerState teamMember) { }
    public void OnPlayerKilled(PlayerState victim, PlayerState killer) { }
    public void OnPlayerRespawn(PlayerState player, ref Vector3 position, ref Quaternion rotation) { }
    public void Restart() { }
    public void Shutdown() { }
    public void Update() { }
}

public class Team
{
    public string name;
    public int score;
}

[DisableAutoCreation]
public class GameModeSystemServer : ComponentSystem
{
    EntityQuery m_PlayersComponentGroup;
    GameWorld _gameWorld;

    public GameModeSystemServer(GameWorld gameWorld) {
        _gameWorld = gameWorld;
    }

    protected override void OnCreate() {
        base.OnCreate();

        m_PlayersComponentGroup = GetEntityQuery(typeof(PlayerState), typeof(PlayerCharacterControl));
    }

    protected override void OnUpdate() {
        var playerStates = m_PlayersComponentGroup.ToComponentArray<PlayerState>();
        var playerEntities = m_PlayersComponentGroup.ToEntityArray(Allocator.TempJob);
        var playerCharacterControls = m_PlayersComponentGroup.ToComponentArray<PlayerCharacterControl>();

        for (int i = 0, c = playerStates.Length; i < c; ++i) {
            var player = playerStates[i];
            var controlledEntity = player.controlledEntity;
            var playerEntity = playerEntities[i];
            var charControl = playerCharacterControls[i];

            // Spawn contolled entity (character) any missing
            if (controlledEntity == Entity.Null) {
                var position = new Vector3(0.0f, 0.2f, 0.0f);
                var rotation = Quaternion.identity;

                CharacterSpawnRequest.Create(PostUpdateCommands, charControl.MechSettings, position, rotation, playerEntity);

                continue;
            }

            //TODO : handle request mech change
        }

        playerEntities.Dispose();
    }
}
