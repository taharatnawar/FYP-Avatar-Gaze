using UnityEngine;

public class AvatarLookIK : MonoBehaviour
{
    public Animator animator;
    public Transform currentLookTarget;

    [Range(0f, 1f)] public float lookWeight = 1f;
    [Range(0f, 1f)] public float bodyWeight = 0.2f;
    [Range(0f, 1f)] public float headWeight = 1f;
    [Range(0f, 1f)] public float eyesWeight = 1f;
    [Range(0f, 1f)] public float clampWeight = 0.5f;

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || currentLookTarget == null)
            return;

        animator.SetLookAtWeight(lookWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
        animator.SetLookAtPosition(currentLookTarget.position);
    }
}