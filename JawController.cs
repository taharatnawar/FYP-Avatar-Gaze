using UnityEngine;

public class JawController : MonoBehaviour
{
    public Transform jawBone;
    public Animator avatarAnimator;

    public float jawOpenAngle = -100f;
    public float jawSpeed = 8f;

    private Quaternion jawClosedRotation;

    void Start()
    {
        jawClosedRotation = jawBone.localRotation;
    }

    void LateUpdate()
    {
        if (jawBone == null) return;

        bool isTalking = avatarAnimator.GetBool("isTalking");
if (isTalking)
{
    float openAmount = (Mathf.Sin(Time.time * jawSpeed) + 1f) / 2f;
    Quaternion openRotation = jawClosedRotation * Quaternion.Euler(0f, 0f, -11f);
    jawBone.localRotation = Quaternion.Lerp(jawClosedRotation, openRotation, openAmount);
}
        else
        {
            jawBone.localRotation = jawClosedRotation;
        }
    }
}
