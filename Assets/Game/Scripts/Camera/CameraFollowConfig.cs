using UnityEngine;


[CreateAssetMenu(fileName = "CameraFollowConfig", menuName = "Camera/FollowConfig")]
public class CameraFollowConfig : ScriptableObject
{
     [Header("Follow Offset")]
    [Tooltip("Local offset from the target, in the target's local space (behind + above).")]
    public Vector3 followOffset = new Vector3(0f, 2f, -6f);
 
    [Header("Smoothing")]
    [Tooltip("Lower = snappier, higher = floatier lag behind the target.")]
    public float positionSmoothTime = 0.25f;
 
    [Tooltip("How fast the camera rotates to match target facing (higher = snappier).")]
    public float rotationSpeed = 4f;
 
    [Header("Collision Avoidance")]
    [Tooltip("Push camera closer if something (terrain, rocks) is between it and the target.")]
    public bool avoidObstacles = true;
    public LayerMask obstacleMask = ~0;
    [Tooltip("Small buffer so the camera doesn't clip into the obstacle surface.")]
    public float obstacleBuffer = 0.3f;
 
    [Header("Field of View (optional 'speed' feel)")]
    [Tooltip("Widen FOV slightly as the jellyfish speeds up, for a sense of motion.")]
    public bool speedFovEnabled = true;
    public float baseFov = 60f;
    public float maxFovBoost = 8f;
    [Tooltip("Speed at which max FOV boost is reached.")]
    public float fovSpeedReference = 12f;
    public float fovSmoothTime = 0.3f;
}
