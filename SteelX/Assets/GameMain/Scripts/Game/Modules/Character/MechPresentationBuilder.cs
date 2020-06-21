using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MechPresentationBuilder
{
    public static void Build(BundledResourceManager resourceManager, MechPresentationSetup mechPresentationSetup, MechSkeleton mechSkeleton, MechSettings mechSettings, bool server) {
        var mechPartRegistry = resourceManager.GetResourceRegistry<MechPartRegistry>();

        // Create new array to store skinned mesh renderers
        SkinnedMeshRenderer[] newSMR = new SkinnedMeshRenderer[4];
        var partPrefabs = new Object[5];

        HeadPartAsset head = mechPartRegistry.heads[mechSettings.Head];
        partPrefabs[0] = resourceManager.GetSingleAssetResource(head.Prefab);

        CorePartAsset core = mechPartRegistry.cores[mechSettings.Core];
        partPrefabs[1] = resourceManager.GetSingleAssetResource(core.Prefab);

        ArmsPartAsset arms = mechPartRegistry.arms[mechSettings.Arms];
        partPrefabs[2] = resourceManager.GetSingleAssetResource(arms.Prefab);

        LegsPartAsset legs = mechPartRegistry.legs[mechSettings.Legs];
        partPrefabs[3] = resourceManager.GetSingleAssetResource(legs.Prefab);

        BoosterPartAsset booster = mechPartRegistry.boosters[mechSettings.Booster];
        partPrefabs[4] = resourceManager.GetSingleAssetResource(booster.Prefab);

        //Extract Skinned Mesh
        for(int i = 0; i < 4; i++) {
            newSMR[i] = ((GameObject)partPrefabs[i]).GetComponentInChildren<SkinnedMeshRenderer>();
        }

        // Replace all
        SkinnedMeshRenderer[] curSMR = mechPresentationSetup.CurSMRs;

        for (int i = 0; i < 4; i++) {//TODO : remake the order part
                                     //Note the order of parts in MechFrame.prefab matters
            if (newSMR[i] == null) { Debug.LogError(i + " is null."); continue; }

            MapBones(mechSkeleton, newSMR[i], curSMR[i]);

            curSMR[i].sharedMesh = newSMR[i].sharedMesh;

            //Material[] mats = new Material[2];
            //mats[0] = Resources.Load("MechPartMaterials/" + partPrefabs[i].name + "mat", typeof(Material)) as Material;
            //mats[1] = Resources.Load("MechPartMaterials/" + partPrefabs[i].name + "_2mat", typeof(Material)) as Material;
            //curSMR[i].materials = mats;

            curSMR[i].enabled = true;
        }

        //Load booster
        Transform boosterbone = mechSkeleton.BoosterBone;
        //Destroy previous booster
        GameObject preBooster = (boosterbone.childCount == 0) ? null : boosterbone.GetChild(0).gameObject;
        if (preBooster != null) {
            GameObject.DestroyImmediate(preBooster);
        }

        GameObject newBooster = GameObject.Instantiate((GameObject)partPrefabs[4], boosterbone);
        newBooster.transform.localPosition = Vector3.zero;
        newBooster.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    private static void MapBones(MechSkeleton mechSkeleton, SkinnedMeshRenderer newPart, SkinnedMeshRenderer partToSwitch) {
        Transform[] MyBones = new Transform[newPart.bones.Length];
        Transform rootBone = mechSkeleton.RootBone;

        for (var i = 0; i < newPart.bones.Length; i++) {
            if (newPart.bones[i].name.Contains(newPart.name)) {
                string boneName = newPart.bones[i].name.Remove(0, 6);
                string boneToFind = "Bip01" + boneName;
                MyBones[i] = TransformExtension.FindDeepChild(rootBone, boneToFind);
            }

            if (MyBones[i] == null) {
                MyBones[i] = TransformExtension.FindDeepChild(rootBone.transform, newPart.bones[i].name, newPart.bones[i].parent.name);
            }

            if (MyBones[i] == null) {
                Transform parent;
                if (newPart.bones[i].parent.name == "Bip01") {//the root bone is not checked
                    rootBone.transform.rotation = Quaternion.identity;
                    parent = rootBone.transform;

                    GameObject newbone = new GameObject(newPart.bones[i].name); //TODO : improve this (mesh on hip has rotation bug , temp. use this to solve)
                    newbone.transform.parent = parent;
                    newbone.transform.localPosition = Vector3.zero;
                    newbone.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    newbone.transform.localScale = new Vector3(1, 1, 1);
                    MyBones[i] = newbone.transform;
                } else {
                    parent = TransformExtension.FindDeepChild(rootBone.transform, newPart.bones[i].parent.name);
                    MyBones[i] = parent;
                }

                if (parent == null) {
                    Debug.LogError("Can't locate the bone : " + newPart.bones[i].name);
                }
            }
        }
        partToSwitch.bones = MyBones;
    }

#if UNITY_EDITOR
    [MenuItem("SteelX/BuildDefaultMech")]
    public static void BuildDefaultMech() {//only need to call this if skinned mesh renderer not render (due to the bones not map to obj's)
        if (Application.isPlaying) return;

        var obj = Selection.activeObject;
        var mechObj = obj as GameObject;
        if (mechObj == null) {
            GameDebug.Log("Not a valid mech");
            return;
        } else {
            var mechSkeleton = mechObj.GetComponentInChildren<MechSkeleton>();
            var mechPresentation = mechObj.GetComponentInChildren<MechPresentationSetup>();

            // Create new array to store skinned mesh renderers
            SkinnedMeshRenderer[] newSMR = new SkinnedMeshRenderer[4];
            var partPrefabs = new Object[5];

            partPrefabs[0] = Resources.Load("Prefabs/DefaultMechParts/d_HDS003");
            partPrefabs[1] = Resources.Load("Prefabs/DefaultMechParts/d_CES301");
            partPrefabs[2] = Resources.Load("Prefabs/DefaultMechParts/d_AES104");
            partPrefabs[3] = Resources.Load("Prefabs/DefaultMechParts/d_LTN411");
            partPrefabs[4] = Resources.Load("Prefabs/DefaultMechParts/d_PTS000");

            //Extract Skinned Mesh
            for (int i = 0; i < 4; i++) {
                newSMR[i] = ((GameObject)partPrefabs[i]).GetComponentInChildren<SkinnedMeshRenderer>();
            }

            // Replace all
            SkinnedMeshRenderer[] curSMR = mechPresentation.CurSMRs;

            for (int i = 0; i < 4; i++) {//TODO : remake the order part
                                         //Note the order of parts in MechFrame.prefab matters
                if (newSMR[i] == null) { Debug.LogError(i + " is null."); continue; }
                MapBones(mechSkeleton, newSMR[i], curSMR[i]);
                curSMR[i].sharedMesh = newSMR[i].sharedMesh;
                curSMR[i].enabled = true;
            }
        }
    }
#endif    

    //    //switch booster aniamtion clips
    //    //if (newBooster != null && newBooster.GetComponent<Animator>() != null && newBooster.GetComponent<Animator>().runtimeAnimatorController != null) {
    //    //    AnimatorOverrideController boosterAnimtor_OC = new AnimatorOverrideController(newBooster.GetComponent<Animator>().runtimeAnimatorController);
    //    //    newBooster.GetComponent<Animator>().runtimeAnimatorController = boosterAnimtor_OC;

    //    //    AnimationClipOverrides boosterClipOverrides = new AnimationClipOverrides(boosterAnimtor_OC.overridesCount);
    //    //    boosterAnimtor_OC.GetOverrides(boosterClipOverrides);

    //    //    boosterClipOverrides["open"] = ((Booster)booster_part).GetOpenAnimation();
    //    //    boosterClipOverrides["close"] = ((Booster)booster_part).GetCloseAnimation();

    //    //    boosterAnimtor_OC.ApplyOverrides(boosterClipOverrides);
    //    //}
    //}

    //    public void Build(GameObject mechFrame, string c, string a, string l, string h, string b, string w1l, string w1r, string w2l, string w2r, int[] skill_IDs) {
    //        MechBones mechBones = mechFrame.GetComponent<MechBones>();

    //        string[] parts = new string[9] { c, a, l, h, b, w1l, w1r, w2l, w2r };


    //        LoadAllPartInfo();

    //        buildSkills(skill_IDs);

    //        //BuildWeapons(mechBones, new string[4] { parts[5], parts[6], parts[7], parts[8] });
    //    }

    //    public void ReplaceMechPart(string toReplace, string newPart) {
    //        Part p = MechPartManager.FindData(newPart);
    //        if (p == null) {
    //            Debug.LogError("Can't find the new part");
    //            return;
    //        }

    //        for (int i = 0; i < 5; i++) {
    //            if (curMechParts[i] != null) {
    //                if (curMechParts[i].name == toReplace) {
    //                    curMechParts[i] = p;
    //                    LoadAllPartInfo();
    //                    return;
    //                }
    //            }
    //        }
    //        Debug.Log("Fail to replace");
    //    }

    //    private void LoadAllPartInfo() {
    //        MechProperty = new MechProperty();

    //        for (int i = 0; i < 5; i++) {
    //            if (curMechParts[i] != null) {
    //                curMechParts[i].LoadPartInfo(ref MechProperty);
    //            }
    //        }
    //    }
}

//public class MechPresentationBuilder
//{
//    //Mech parts
//    public Part[] curMechParts = new Part[5];

//    //Mech animator settings
//    private Animator animator;
//    private AnimatorOverrideController animatorOverrideController;
//    private AnimationClipOverrides clipOverrides;
//    private MovementClips defaultMovementClips, TwoHandedMovementClips;

//    public bool onPanel = false;

//    public delegate void BuildWeaponAction();
//    public event BuildWeaponAction OnMechBuilt;









//    private void BuildWeapons(MechBones mechBones, string[] weaponNames) {
//        Transform[] hands = mechBones.Hands;
//        //Destroy previous weapons
//        if (Weapons != null) {
//            for (int i = 0; i < Weapons.Length; i++) {
//                WeaponDatas[i] = null;
//                if (Weapons[i] != null) {
//                    Weapons[i].OnDestroy();
//                    Weapons[i] = null;
//                }
//            }
//        }

//        //Find and create corresponding weapon script
//        for (int i = 0; i < WeaponDatas.Length; i++) {
//            WeaponDatas[i] = (i >= weaponNames.Length || weaponNames[i] == "Empty" || string.IsNullOrEmpty(weaponNames[i])) ? null : WeaponDataManager.FindData(weaponNames[i]);

//            if (WeaponDatas[i] == null) {
//                if (i < weaponNames.Length && (weaponNames[i] != "Empty" || string.IsNullOrEmpty(weaponNames[i])))
//                    Debug.LogError("Can't find weapon data : " + weaponNames[i]);
//                continue;
//            }

//            Weapons[i] = (Weapon)WeaponDatas[i].GetWeaponObject();
//        }

//        //Init weapon scripts
//        for (int i = 0; i < WeaponDatas.Length; i++) {
//            if (WeaponDatas[i] == null) continue;
//            Transform weapPos = (WeaponDatas[i].IsTwoHanded) ? hands[(i + 1) % 2] : hands[i % 2];
//            Weapons[i].Init(WeaponDatas[i], i, weapPos, MechCombat, animator);
//        }

//        //if (buildLocally) UpdateAnimatorState();

//        //Enable renderers
//        for (int i = 0; i < Weapons.Length; i++) {
//            if (Weapons[i] == null) continue;
//            Weapons[i].ActivateWeapon((i == weaponOffset || i == weaponOffset + 1));
//        }
//    }

//    private void buildSkills(int[] skill_IDs) {
//        if (skill_IDs == null) { Debug.Log("skill_IDs is null"); return; }
//        SkillConfig[] skills = new SkillConfig[4];
//        for (int i = 0; i < skill_IDs.Length; i++) {
//            skills[i] = SkillManager.GetSkillConfig(skill_IDs[i]);
//        }

//        if (SkillController != null) SkillController.SetSkills(skills);
//    }

//    public void EquipWeapon(string weapon, int pos) {
//        //WeaponData data = WeaponDataManager.FindData(weapon);
//        //if (data == null) { Debug.LogError("Can't find weapon data : " + weapon); return; }

//        ////if previous is two-handed => also destroy left hand
//        //if (pos == 3) {
//        //    if (Weapons[2] != null && WeaponDatas[2].IsTwoHanded) {
//        //        if (isDataGetSaved) PlayerData.myData.Mech[Mech_Num].Weapon2L = "Empty";

//        //        Weapons[2].OnDestroy();
//        //        Weapons[2] = null;
//        //        WeaponDatas[2] = null;
//        //    }
//        //} else if (pos == 1) {
//        //    if (Weapons[0] != null && WeaponDatas[0].IsTwoHanded) {
//        //        if (isDataGetSaved) PlayerData.myData.Mech[Mech_Num].Weapon1L = "Empty";

//        //        Weapons[0].OnDestroy();
//        //        Weapons[0] = null;
//        //        WeaponDatas[0] = null;
//        //    }
//        //}

//        ////if the new one is two-handed => also destroy right hand
//        //if (data.IsTwoHanded) {
//        //    if (Weapons[pos + 1] != null) {
//        //        Weapons[pos + 1].OnDestroy();
//        //        Weapons[pos + 1] = null;
//        //    }

//        //    if (pos == 0) {
//        //        if (isDataGetSaved) PlayerData.myData.Mech[Mech_Num].Weapon1R = "Empty";
//        //        WeaponDatas[1] = null;
//        //    } else if (pos == 2) {
//        //        if (isDataGetSaved) PlayerData.myData.Mech[Mech_Num].Weapon2R = "Empty";
//        //        WeaponDatas[3] = null;
//        //    }
//        //}

//        ////destroy the current weapon on the hand position
//        //if (Weapons[pos] != null) {
//        //    Weapons[pos].OnDestroy();
//        //    Weapons[pos] = null;
//        //}

//        ////Init
//        //WeaponDatas[pos] = data;
//        //Weapons[pos] = (Weapon)(WeaponDatas[pos].GetWeaponObject());
//        //Transform weapPos = (WeaponDatas[pos].IsTwoHanded) ? hands[(pos + 1) % 2] : hands[pos % 2];
//        //Weapons[pos].Init(WeaponDatas[pos], pos % 2, weapPos, MechCombat, animator);

//        //Weapons[pos].ActivateWeapon(pos == weaponOffset || pos == weaponOffset + 1);

//        //UpdateAnimatorState();

//        //if (buildLocally) {
//        //    MechIK.UpdateMechIK(weaponOffset);
//        //} else {
//        //    UpdateMechCombatVars();
//        //}

//        ////Display properties
//        //if (!onPanel && OperatorStatsUI != null) {//!onPanel : hargar,lobby,store
//        //    OperatorStatsUI.DisplayMechProperties();
//        //}
//    }

//    public void DisplayWeapons(int weaponOffset) {
//        this.weaponOffset = weaponOffset;
//        for (int i = 0; i < 4; i++) if (Weapons[i] != null) Weapons[i].ActivateWeapon(i == weaponOffset || i == weaponOffset + 1);

//        if (!onPanel && OperatorStatsUI != null) {//!onPanel : hargar,lobby,store
//            OperatorStatsUI.DisplayMechProperties();
//        }
//    }

//    public int GetWeaponOffset() {
//        return weaponOffset;
//    }

//    public void UpdateAnimatorState() {
//        if (animator == null) { Debug.LogError("Animator is null"); return; };

//        MovementClips movementClips = (WeaponDatas[weaponOffset] != null && WeaponDatas[weaponOffset].IsTwoHanded) ? TwoHandedMovementClips : defaultMovementClips;
//        for (int i = 0; i < movementClips.clips.Length; i++) {
//            clipOverrides[movementClips.clipnames[i]] = movementClips.clips[i];
//        }
//        animatorOverrideController.ApplyOverrides(clipOverrides);
//    }

//    private void UpdateMechCombatVars() {
//        if (MechCombat == null) return;

//        if (OnMechBuilt != null) OnMechBuilt();
//        if (MechCombat.OnWeaponSwitched != null) MechCombat.OnWeaponSwitched();

//        MechCombat.EnableAllRenderers(true);//TODO : check this
//        MechCombat.EnableAllColliders(true);
//    }

//    public PhotonPlayer GetOwner() {
//        return null;
//    }
//}

///*
//     for (int i = 0; i < parts.Length - 4; i++) {
//            Debug.LogWarning("part "+ i + " is null");
//            parts[i] = string.IsNullOrEmpty(parts[i]) ? defaultParts[i] : parts[i];
//        }

//        //set weapons if null (in offline)
//        if (string.IsNullOrEmpty(parts[5])) parts[5] = defaultParts[6];
//        if (string.IsNullOrEmpty(parts[6])) parts[6] = defaultParts[15];
//        if (string.IsNullOrEmpty(parts[7])) parts[7] = defaultParts[10];
//        if (string.IsNullOrEmpty(parts[8])) parts[8] = defaultParts[16];

//        if (skill_IDs == null) {//TODO : remake this
//            Debug.Log("skill_ids is null. Set defualt skills");
//            SkillManager.GetAllSkills();
//            skill_IDs = new int[4] { 0, 1, 3, 4 };
//        }*/

///*if (!buildLocally) {
//            UpdateMechCombatVars();
//        } else if (!onPanel) {//display properties //todo : move this elsewhere
//            OperatorStatsUI OperatorStatsUI = FindObjectOfType<OperatorStatsUI>();
//            if (OperatorStatsUI != null) {
//                OperatorStatsUI.DisplayMechProperties();
//            }
//        }*/
////if (_owner.IsLocal && !PhotonNetwork.isMasterClient){
////photonView.ObservedComponents.Clear();
////photonView.synchronization = ViewSynchronization.Off;
////}

////private void LoadMovementClips() {
////    defaultMovementClips = Resources.Load<MovementClips>("Data/MovementClip/Default");
////    TwoHandedMovementClips = Resources.Load<MovementClips>("Data/MovementClip/TwoHanded");
////}

////private void InitComponents() {
////Transform CurrentMech = transform.Find("CurrentMech");
////AnimatorHashVars.HashAnimatorVars();
////}


////private void InitAnimatorControllers() {//do not call this in game otherwise mechcombat gets null parameter
////    if (!buildLocally) return;

////    animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
////    animator.runtimeAnimatorController = animatorOverrideController;

////    clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
////    animatorOverrideController.GetOverrides(clipOverrides);//write clips into clipOverrides
////}