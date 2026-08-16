

using System;
using UnityEngine;

public class JellyGrowthController : MonoBehaviour
{
    public JellyGrowthConfig jellyGrowth;
    public JellyFishMouth mouth;
    public Transform visualRootPos;
    public float scaleLerpSpeed = 4f;
    public Animator anim;
    public JellyfishController jellyControls;
    public CameraFollow cameraFollow;
    public Transform mouthTransform;
    public float mouthScaleMultiplier = 1f;
    public Vector3 baseMouthScale = Vector3.one;

    public bool logStageChangeDebug = true;

    public int totalExp {get; private set;}
    public int currentStageIndex {get; private set;}
    public JellyFishStage currentFishStage => IsValidIndex(currentStageIndex) ? jellyGrowth.stages[currentStageIndex] : null;
    public JellyFishStage nextFishStage => IsValidIndex(currentStageIndex + 1) ? jellyGrowth.stages[currentStageIndex + 1]: null;
    public bool IsMaxStage => nextFishStage == null;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(visualRootPos == null) return;

        float targetScale = Mathf.Lerp(currentFishStage.targetScale, IsMaxStage ? currentFishStage.targetScale : nextFishStage.targetScale, nextStageProgress);

        currentAppliedScale = Mathf.Lerp(currentAppliedScale, targetScale, 1f - Mathf.Exp(-scaleLerpSpeed * Time.deltaTime));

        visualRootPos.localScale = Vector3.one * currentAppliedScale;

        if(mouthTransform != null)
        {
            mouthTransform.localScale = baseMouthScale * currentAppliedScale * mouthScaleMultiplier;
        }
    }

    public void AddXp(int amount)
    {
        if(amount <= 0) return;

        totalExp += amount;
        OnXpGained?.Invoke(amount, totalExp);

        while(!IsMaxStage && totalExp >= nextFishStage.xpThreshold)
        {
            currentStageIndex++;
            Evolve(currentFishStage);
        }
    }

    void Evolve(JellyFishStage newStage)
    {
        if(logStageChangeDebug)
        {
            Debug.Log($"[JellyGrowthController] Evolved to stage {currentStageIndex}: {newStage.stageName}");
        }
        //Stage visual func goes here
        ApplyStageVisualAndSettings(newStage, playFeedback: true);
        OnStageChanged?.Invoke(newStage, currentStageIndex);
    }

    void ApplyStageVisualAndSettings(JellyFishStage stage, bool playFeedback)
    {
        if(stage.visualPrefab != null && visualRootPos != null)
        {
            if(currentVisualMesh != null)
            {
                Destroy(currentVisualMesh);
            }
            currentVisualMesh = Instantiate(stage.visualPrefab, visualRootPos);
            currentVisualMesh.transform.localPosition = Vector3.zero;
            currentVisualMesh.transform.localRotation = Quaternion.identity;

            Animator swappedAnimator = currentVisualMesh.GetComponentInChildren<Animator>();
            currentAnim = swappedAnimator != null ? swappedAnimator : anim;

            if(jellyControls != null)
            {
                jellyControls.SetAnimator(currentAnim);
            }
        }

        if(stage.movementConfigOverride != null && jellyControls != null)
        {
            jellyControls.GenerateNewConfig(stage.movementConfigOverride);
        }
        if(stage.cameraConfigOverride != null && cameraFollow != null)
        {
            cameraFollow.config = stage.cameraConfigOverride;
        }

        if(playFeedback)
        {
            if(stage.evolveVFXPrefab != null)
            {
                GameObject vfx = Instantiate(stage.evolveVFXPrefab, transform.position, Quaternion.identity);
                Destroy(vfx, 3f);
            }

            if(stage.evolveSFX != null)
            {
                AudioSource.PlayClipAtPoint(stage.evolveSFX, transform.position);
            }

            if(currentAnim != null && !string.IsNullOrEmpty(stage.evolveAnimTrigger))
            {
                currentAnim.SetTrigger(stage.evolveAnimTrigger);
            }
        }
    }

    public float nextStageProgress
    {
        get
        {
            if(IsMaxStage) return 1f;
            int span = nextFishStage.xpThreshold - currentFishStage.xpThreshold;
            if(span <= 0) return 1f;
            return Mathf.Clamp01((float)(totalExp - currentFishStage.xpThreshold)/span);
        } 
    }



    // Events

    public event Action<int, int> OnXpGained;
    public event Action<JellyFishStage, int> OnStageChanged;

    GameObject currentVisualMesh;
    float currentAppliedScale;


    Animator currentAnim;


    void Awake()
    {
        if(jellyGrowth == null || jellyGrowth.stages == null || jellyGrowth.stages.Count == 0)
        {
            Debug.LogError("[JellyGrowthController] No growth config / stages assigned.", this);
            enabled = false;
            return;
        }

        currentStageIndex = 0;
        totalExp = 0;
        currentAnim = anim;

        if(mouthTransform != null)
        {
            baseMouthScale = mouthTransform.localScale;
        }

        if (visualRootPos != null && visualRootPos.childCount > 0)
    {
        currentVisualMesh = visualRootPos.GetChild(0).gameObject;
    }

        currentAppliedScale = currentFishStage.targetScale;
        if(visualRootPos != null)
        {
            visualRootPos.localScale = Vector3.one * currentAppliedScale;

        }
    }

    void OnEnable()
    {
        if(mouth != null)
        {
            mouth.OnFoodEaten += AddXp;
        }
    }

    void OnDisable()
    {
        if(mouth != null)
        {
            mouth.OnFoodEaten -= AddXp;
        }
    }
    public bool IsValidStageIndex(int index) => IsValidIndex(index);

  bool IsValidIndex(int index)
{
    return jellyGrowth != null && jellyGrowth.stages != null 
        && index >= 0 && index < jellyGrowth.stages.Count;
}
}
