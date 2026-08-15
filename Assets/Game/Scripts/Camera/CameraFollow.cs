using UnityEngine;

public class CameraFollow : MonoBehaviour
{
     [Header("Refs")]
    [Tooltip("The jellyfish transform to follow.")]
    public Transform target;
    [Tooltip("Optional - used to read speed for the FOV effect. Leave empty to disable.")]
    public JellyfishController targetMovement;
    public CameraFollowConfig config;
 
    Camera cam;
    Vector3 positionVelocity; // used by SmoothDamp
    float fovVelocity;
 
    void Awake()
    {
        cam = GetComponent<Camera>();
        if (config != null)
        {
            cam.fieldOfView = config.baseFov;
        }
    }
 
    void LateUpdate()
    {
        if (target == null || config == null) return;
 
        FollowPosition();
        FollowRotation();
 
        if (config.speedFovEnabled && targetMovement != null)
        {
            UpdateSpeedFov();
        }
    }
 
    void FollowPosition()
    {
        // Desired position is the offset transformed into target's local space,
        // so the camera stays behind the jellyfish no matter which way it's facing.
        Vector3 desiredPosition = target.TransformPoint(config.followOffset);
 
        if (config.avoidObstacles)
        {
            desiredPosition = ResolveObstacles(target.position, desiredPosition);
        }
 
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref positionVelocity,
            config.positionSmoothTime
        );
    }
 
    void FollowRotation()
    {
        Quaternion desiredRotation = Quaternion.LookRotation(
            (target.position + target.forward * 5f) - transform.position,
            target.up
        );
 
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            1f - Mathf.Exp(-config.rotationSpeed * Time.deltaTime)
        );
    }
 
    Vector3 ResolveObstacles(Vector3 from, Vector3 desired)
    {
        Vector3 direction = desired - from;
        float distance = direction.magnitude;
 
        if (Physics.Raycast(from, direction.normalized, out RaycastHit hit, distance, config.obstacleMask, QueryTriggerInteraction.Ignore))
        {
            // Pull the camera in to just before the obstacle surface
            float safeDistance = Mathf.Max(0f, hit.distance - config.obstacleBuffer);
            return from + direction.normalized * safeDistance;
        }
 
        return desired;
    }
 
    void UpdateSpeedFov()
    {
        float speedT = Mathf.Clamp01(targetMovement.currentSpeed / config.fovSpeedReference);
        float targetFov = config.baseFov + config.maxFovBoost * speedT;
 
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFov, ref fovVelocity, config.fovSmoothTime);
    }
}
