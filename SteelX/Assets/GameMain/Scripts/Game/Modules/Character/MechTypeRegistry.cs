using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SteelX/Mech/MechTypeRegistry", fileName = "MechTypeRegistry")]
public class MechTypeRegistry : RegistryBase
{
    public List<MechTypeAsset> entries = new List<MechTypeAsset>();

#if UNITY_EDITOR

    public override void PrepareForBuild() {
        Debug.Log("MechTypeRegistry");

        entries.Clear();
        var guids = AssetDatabase.FindAssets("t:MechTypeAsset");
        foreach (var guid in guids) {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var definition = AssetDatabase.LoadAssetAtPath<MechTypeAsset>(path);
            Debug.Log("   Adding definition:" + definition);
            entries.Add(definition);
        }

        EditorUtility.SetDirty(this);
    }

    public override void GetSingleAssetGUIDs(List<string> guids, bool serverBuild) {
        foreach (var setup in entries) {

            if (serverBuild && setup.Mech.prefabServer.IsSet())
                guids.Add(setup.Mech.prefabServer.GetGuidStr());
            if (!serverBuild && setup.Mech.prefabClient.IsSet())
                guids.Add(setup.Mech.prefabClient.GetGuidStr());
        }
    }
#endif
}

