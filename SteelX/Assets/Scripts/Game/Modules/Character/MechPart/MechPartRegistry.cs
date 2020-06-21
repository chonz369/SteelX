using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SteelX/Mech/MechPartRegistry", fileName = "MechPartRegistry")]
public class MechPartRegistry : RegistryBase
{
    public List<HeadPartAsset> heads = new List<HeadPartAsset>();
    public List<CorePartAsset> cores = new List<CorePartAsset>();
    public List<ArmsPartAsset> arms = new List<ArmsPartAsset>();
    public List<LegsPartAsset> legs = new List<LegsPartAsset>();
    public List<BoosterPartAsset> boosters = new List<BoosterPartAsset>();

#if UNITY_EDITOR

    public override void PrepareForBuild() {
        Debug.Log("MechPartRegistry");

        PrepareAsset(heads, typeof(HeadPartAsset).ToString());
        PrepareAsset(cores, typeof(CorePartAsset).ToString());
        PrepareAsset(arms, typeof(ArmsPartAsset).ToString());
        PrepareAsset(legs, typeof(LegsPartAsset).ToString());
        PrepareAsset(boosters, typeof(BoosterPartAsset).ToString());

        EditorUtility.SetDirty(this);
    }

    private void PrepareAsset<T>(List<T> entries, string assetType) where T : MechPartAssetBase{
        entries.Clear();
        var guids = AssetDatabase.FindAssets("t:" + assetType);
        foreach (var guid in guids) {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var definition = AssetDatabase.LoadAssetAtPath<T>(path);
            Debug.Log("   Adding definition:" + definition);
            entries.Add(definition);
        }
    }

    public override void GetSingleAssetGUIDs(List<string> guids, bool serverBuild) {
        foreach(var part in heads) {
            guids.Add(part.Prefab.GetGuidStr());
        }

        foreach (var part in cores) {
            guids.Add(part.Prefab.GetGuidStr());
        }

        foreach (var part in arms) {
            guids.Add(part.Prefab.GetGuidStr());
        }

        foreach (var part in legs) {
            guids.Add(part.Prefab.GetGuidStr());
        }

        foreach (var part in boosters) {
            guids.Add(part.Prefab.GetGuidStr());
        }
    }
#endif
}

