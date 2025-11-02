using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] Transform targetPose;

    [Header("Animation Setup")]
    [SerializeField] private float animationsSpeed = .5f;
    [SerializeField] private float leanDegree = 30f;
    [SerializeField] private float JumpScaleMultiplier = .5f;

    private Transform starterPosition;
    private int direction = 1;

    private void Start()
    {
        starterPosition = root;
    }
    private void Update()
    {
        root.rotation = Quaternion.Lerp(root.rotation, targetPose.transform.rotation, animationsSpeed);
        root.localScale = Vector3.Lerp(root.localScale, targetPose.localScale, animationsSpeed);
        if (direction * rb.linearVelocity.x < 0)
        {
            direction *= -1;
            Rotate();
        }
        if (Mathf.Abs(rb.linearVelocity.x) > float.Epsilon)
            Lean();
        else
            StopLean();

        if (Mathf.Abs(rb.angularVelocity.y) > float.Epsilon)
            Jumping();
        else
            StopJumping();

    }
    private void Lean()
    {
        targetPose.rotation = new Quaternion(root.rotation.x, root.rotation.y, leanDegree, root.rotation.w);
    }
    private void StopLean()
    {
        targetPose.rotation = starterPosition.transform.rotation;
    }
    private void Jumping()
    {
        targetPose.localScale = new Vector3(root.localScale.x, root.localScale.y * JumpScaleMultiplier);
    }
    private void StopJumping()
    {
        targetPose.localScale = starterPosition.localScale;
    }
    private void Rotate()
    {
        targetPose.rotation = new Quaternion(root.rotation.x, root.rotation.y, root.rotation.z + 180, root.rotation.w);
    }
}
