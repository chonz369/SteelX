using UnityEngine;

public static class TransformExtension
{
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName, string ParentMustMatch = "") {
        var result = aParent.Find(aName);

        if (result != null && (ParentMustMatch == "" || ParentMustMatch == result.parent.name))
            return result;

        foreach (Transform child in aParent) {
            result = child.FindDeepChild(aName, ParentMustMatch);
            if (result != null) {
                return result;
            }
        }
        return null;
    }

    public static Vector3 RandomXZposition(Vector3 pos, float radius) {
        float x = Random.Range(pos.x - radius, pos.x + radius);
        float z = Random.Range(pos.z - radius, pos.z + radius);
        return new Vector3(x, pos.y, z);
    }

    public static void SetLocalTransform(Transform t, Vector3 pos = default(Vector3), Quaternion rot = default(Quaternion), Vector3 scale = default(Vector3)) {
        t.localPosition = pos;
        t.localRotation = rot;

        if (scale == default(Vector3)) {
            return;
        } else
            t.localScale = scale;
    }
}