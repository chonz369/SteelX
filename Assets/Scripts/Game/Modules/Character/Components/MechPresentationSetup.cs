using System;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class MechPresentationSetup : MonoBehaviour
{
    public GameObject Geomtry;
    public Transform WeaponAttachBoneL, WeaponAttachBoneR;
    public Transform BoosterBone;
    public Transform ItemAttachBone;

    public SkinnedMeshRenderer[] CurSMRs = new SkinnedMeshRenderer[4];

    [NonSerialized] public Entity character;
    [NonSerialized] public bool updateTransform = true;
    [NonSerialized] public Entity attachToPresentation;

    public bool IsVisible {
        get { return isVisible; }
    }

    public void SetVisible(bool visible) {
        isVisible = visible;
        if (Geomtry != null && Geomtry.activeSelf != visible)
            Geomtry.SetActive(visible);
    }

    [NonSerialized] bool isVisible = true;
}

[DisableAutoCreation]
public class UpdatePresentationRootTransform : BaseComponentSystem<MechPresentationSetup>
{
    private EntityQuery Group;

    public UpdatePresentationRootTransform(GameWorld world) : base(world) { }

    protected override void Update(Entity entity, MechPresentationSetup charPresentation) {
        if (!charPresentation.updateTransform)
            return;

        if (charPresentation.attachToPresentation != Entity.Null)
            return;

        var animState = EntityManager.GetComponentData<CharacterInterpolatedData>(charPresentation.character);
        charPresentation.transform.position = animState.position;
        charPresentation.transform.rotation = Quaternion.Euler(0f, animState.rotation, 0f);
    }
}

//[DisableAutoCreation]
//public class UpdatePresentationAttachmentTransform : BaseComponentSystem<CharacterPresentationSetup>
//{
//    public UpdatePresentationAttachmentTransform(GameWorld world) : base(world) { }

//    protected override void Update(Entity entity, CharacterPresentationSetup charPresentation) {
//        if (!charPresentation.updateTransform)
//            return;

//        if (charPresentation.attachToPresentation == Entity.Null)
//            return;


//        if (!EntityManager.Exists(charPresentation.attachToPresentation)) {
//            GameDebug.LogWarning("Huhb ?");
//            return;
//        }

//        var refPresentation =
//            EntityManager.GetComponentObject<CharacterPresentationSetup>(charPresentation.attachToPresentation);

//        charPresentation.transform.position = refPresentation.itemAttachBone.position;
//        charPresentation.transform.rotation = refPresentation.itemAttachBone.rotation;

//    }
//}