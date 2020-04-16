using UnityEngine;

public class MechSkeleton : MonoBehaviour
{
    public Transform RootBone;
    public Transform BoosterBone;

    public Transform leftToeBone, rightToeBone;

    private void Awake() {
        Animator animator = GetComponent<Animator>();
        if (animator == null) return;

        leftToeBone = animator.GetBoneTransform(HumanBodyBones.LeftToes);
        rightToeBone = animator.GetBoneTransform(HumanBodyBones.RightToes);
    }
}

