using UnityEngine;
using Unity.Entities;

[RequireComponent(typeof(LocalPlayer))]
[RequireComponent(typeof(PlayerCameraSettings))]
public class LocalPlayerCharacterControl : MonoBehaviour
{
    [ConfigVar(Name = "char.showhistory", DefaultValue = "0", Description = "Show last char loco states")]
    public static ConfigVar ShowHistory;

    public Entity lastRegisteredControlledEntity;

    public int lastDamageInflictedTick;
    public int lastDamageReceivedTick;
}

[DisableAutoCreation]
public class UpdateCharacterCamera : BaseComponentSystem<LocalPlayer, LocalPlayerCharacterControl, PlayerCameraSettings>
{
    private const float k_default3PDisst = 2.5f;
    private float camDist3P = k_default3PDisst;

    public UpdateCharacterCamera(GameWorld world) : base(world) { }

    public void ToggleFOrceThirdPerson() {
        forceThirdPerson = !forceThirdPerson;
    }

    protected override void Update(Entity entity, LocalPlayer localPlayer, LocalPlayerCharacterControl characterControl, PlayerCameraSettings cameraSettings) {
        if (localPlayer.controlledEntity == Entity.Null || !EntityManager.HasComponent<Character>(localPlayer.controlledEntity)) {
            controlledEntity = Entity.Null;
            return;
        }

        GameDebug.Assert(EntityManager.HasComponent<CharacterInterpolatedData>(localPlayer.controlledEntity), "Controlled entity has no animstate");

        var character = EntityManager.GetComponentObject<Character>(localPlayer.controlledEntity);
        var charPredictedState = EntityManager.GetComponentData<CharacterPredictedData>(localPlayer.controlledEntity);

        var animState = EntityManager.GetComponentData<CharacterInterpolatedData>(localPlayer.controlledEntity);

        // Check if this is first time update is called with this controlled entity
        var characterChanged = localPlayer.controlledEntity != controlledEntity;
        if (characterChanged) {
            controlledEntity = localPlayer.controlledEntity;
        }

        // Update camera settings
        var userCommand = EntityManager.GetComponentData<UserCommandComponentData>(localPlayer.controlledEntity);
        var lookRotation = userCommand.command.lookRotation;

        cameraSettings.isEnabled = true;

        var eyePos = charPredictedState.position + Vector3.up * 1.8f;//TODO : fix this constant
        cameraSettings.position = eyePos;
        cameraSettings.rotation = lookRotation;

        // Simpe offset of camera for better 3rd person view. This is only for animation debug atm
        var viewDir = cameraSettings.rotation * Vector3.forward;
        cameraSettings.position += -camDist3P * viewDir;
        cameraSettings.position += lookRotation * Vector3.right * 0.5f + lookRotation * Vector3.up * 0.5f;
    }

    bool forceThirdPerson;
    Entity controlledEntity;
}
