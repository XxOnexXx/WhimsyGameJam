using UnityEngine;
public class JiggleBone : MonoBehaviour
{
    [Header("Spring Feel")]
    [Tooltip("How strongly this bone is pulled back toward its resting local rotation relative to its parent. Higher = stiffer, snaps back faster.")]
    [Range(0f, 50f)] public float stiffness = 12f;

    [Tooltip("How much the motion is damped/slowed each frame. Higher = less wobble, settles faster. Too low = infinite jiggling.")]
    [Range(0f, 20f)] public float damping = 6f;

    [Header("Motion Influence")]
    [Tooltip("How much the PARENT's movement/rotation this frame pushes this bone off its resting pose. Higher = more dramatic trailing effect.")]
    [Range(0f, 2f)] public float motionInfluence = 0.6f;

    [Tooltip("Simple downward droop applied constantly, like gravity/water resistance pulling tentacle tips down and back.")]
    public float gravityDroop = 0f;

    [Header("Limits")]
    [Tooltip("Max degrees this bone can deviate from its resting local rotation, in any axis. Prevents tentacles flipping inside out on sudden fast movement.")]
    public float maxDeviationAngle = 45f;

    // Internal simulation state
    Quaternion restLocalRotation;
    Quaternion currentVelocityRotation = Quaternion.identity; 
    Vector3 previousParentPosition;
    Quaternion previousParentRotation;

    void Start()
    {
        restLocalRotation = transform.localRotation;

        if (transform.parent != null)
        {
            previousParentPosition = transform.parent.position;
            previousParentRotation = transform.parent.rotation;
        }
    }

    void LateUpdate()
    {
        if (transform.parent == null) return;

        Vector3 parentPositionDelta = transform.parent.position - previousParentPosition;
        Quaternion parentRotationDelta = transform.parent.rotation * Quaternion.Inverse(previousParentRotation);

        previousParentPosition = transform.parent.position;
        previousParentRotation = transform.parent.rotation;

        // Convert parent's positional movement into a small rotational push on this bone
        // (a tentacle tip lags behind when the base suddenly moves sideways/forward).
        Vector3 localMotionPush = transform.parent.InverseTransformDirection(parentPositionDelta) * motionInfluence;
        Quaternion motionPushRotation = Quaternion.Euler(
            -localMotionPush.z * 40f,  // forward/back motion -> pitch
             localMotionPush.x * 40f,  // sideways motion -> yaw
             0f
        );

        // Target = resting pose, nudged by parent's rotation delta and motion push.
        Quaternion targetRotation = restLocalRotation * motionPushRotation;

        if (gravityDroop != 0f)
        {
            targetRotation *= Quaternion.Euler(gravityDroop, 0f, 0f);
        }

        // Spring toward target: current velocity accumulates based on how far we are
        Quaternion toTarget = targetRotation * Quaternion.Inverse(transform.localRotation);
        toTarget.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f; // shortest-path angle

        Quaternion springForce = Quaternion.AngleAxis(angle * stiffness * Time.deltaTime, axis);
        currentVelocityRotation = Quaternion.Slerp(Quaternion.identity, currentVelocityRotation * springForce, 1f - damping * Time.deltaTime);

        Quaternion newLocalRotation = currentVelocityRotation * transform.localRotation;

        // Clamp deviation from rest pose so fast movement can't fully invert the bone
        float deviation = Quaternion.Angle(restLocalRotation, newLocalRotation);
        if (deviation > maxDeviationAngle)
        {
            newLocalRotation = Quaternion.Slerp(restLocalRotation, newLocalRotation, maxDeviationAngle / deviation);
        }

        transform.localRotation = newLocalRotation;
    }
}