using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "JellyFishGrowthConfig", menuName = "Jelly/Growth Config")]
public class JellyGrowthConfig : ScriptableObject
{
     [Tooltip("Stages in ascending order of xpThreshold. Stage 0 should have xpThreshold = 0.")]
    public List<JellyFishStage> stages = new List<JellyFishStage>();
 
#if UNITY_EDITOR
    void OnValidate()
    {
        for (int i = 1; i < stages.Count; i++)
        {
            if (stages[i] != null && stages[i - 1] != null && stages[i].xpThreshold < stages[i - 1].xpThreshold)
            {
                Debug.LogWarning($"[JellyGrowthConfig] '{name}': stage '{stages[i].stageName}' has a lower xpThreshold than the previous stage. Stages should be in ascending order.", this);
            }
        }
    }
#endif
}
