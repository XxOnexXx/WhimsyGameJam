using UnityEngine;

[CreateAssetMenu(fileName= "JellyFishGrowthStage", menuName = "JellyGrowth/Growth Stage")]
public class JellyFishStage : ScriptableObject
{
     [Header("Identity")]
    [Tooltip("Just for readability in the inspector/logs, not used for logic.")]
    public string stageName = "Stage";
 
    [Header("Progression")]
    [Tooltip("Cumulative XP required to REACH this stage. Stage 0 should be 0.")]
    public int xpThreshold = 0;
 
    [Header("Visual Scale")]
    [Tooltip("Uniform scale applied once fully at this stage. Growth lerps toward this as XP approaches the NEXT stage's threshold.")]
    public float targetScale = 1f;
 
    [Header("Visual Swap (optional)")]
    [Tooltip("If set, this prefab replaces the current visual model on reaching this stage (discrete swap, e.g. new mesh/rig for a bigger jellyfish). Leave empty to just keep scaling the existing model.")]
    public GameObject visualPrefab;
 
    [Header("Behavior Overrides (optional)")]
    [Tooltip("If set, jellyfish movement feel switches to this config on reaching this stage (e.g. bigger = slower turn, stronger pulse).")]
    public JellyConfig movementConfigOverride;
 
    [Tooltip("If set, camera follow feel switches to this config on reaching this stage (e.g. pulls back further for a bigger jellyfish).")]
    public CameraFollowConfig cameraConfigOverride;
 
    [Header("Evolve Feedback (optional)")]
    public GameObject evolveVFXPrefab;
    public AudioClip evolveSFX;
 
    [Tooltip("Animator trigger name fired on evolving INTO this stage. Leave empty to skip.")]
    public string evolveAnimTrigger = "Evolve";
}
