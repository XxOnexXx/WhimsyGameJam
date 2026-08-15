
using Unity.Mathematics;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class JellyfishController : MonoBehaviour
{
    [Header("Refs")]

    public PlayerInput input;
    public JellyConfig config;


    [Header("Animations")]
    Animator anim;
    public string pulseAnimTrigger = "Pulse";


    Rigidbody rb;
    float yaw;
    float pitch;
    float lastPulseTime = -999f;
    float idleBobTimer;

    public float currentSpeed => rb.linearVelocity.magnitude;
    public bool canPulse => Time.time > lastPulseTime + config.pulseCooldown;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 0f;
        anim = GetComponent<Animator>();

        Vector3 startingEuler = transform.eulerAngles;
        yaw = startingEuler.y;
        pitch = startingEuler.x;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        HandleLookCamera();

        if(input.pulseInput && canPulse)
        {
            Pulse();
        }
    }

    void FixedUpdate()
    {
        HandleBouyancy();
        HandleWaterDrag();
        ClampMaxPlayerSpeed();
        IdleBobAnimation();
    }

    void HandleLookCamera()
    {
        yaw += input.lookDir.x * config.lookSensitivity;
        pitch -= input.lookDir.y * config.lookSensitivity;
        pitch = math.clamp(pitch, -config.pitchClampAngle, config.pitchClampAngle);

        Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - Mathf.Exp(-config.rotationSpeed * Time.deltaTime));
    }

    void Pulse()
    {
        lastPulseTime = Time.time;
        rb.AddForce(transform.forward * config.pulseForce, ForceMode.VelocityChange);

        if(anim != null && !string.IsNullOrEmpty(pulseAnimTrigger))
        {
            anim.SetTrigger(pulseAnimTrigger);
        }
    }

    void HandleBouyancy()
    {
        rb.AddForce(Vector3.up * config.buoyancyForce, ForceMode.Acceleration);
    }

    void HandleWaterDrag()
    {   
        rb.linearVelocity *= Mathf.Clamp01(1f - config.waterDrag * Time.fixedDeltaTime);
    }

    void ClampMaxPlayerSpeed()
    {
        if(rb.linearVelocity.magnitude > config.maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * config.maxSpeed;
        }
    }

    void IdleBobAnimation()
    {
        if(rb.linearVelocity.magnitude < 0.1f)
        {
            idleBobTimer += Time.fixedDeltaTime * config.idleBobSpeed;
            float bob = Mathf.Sin(idleBobTimer) * config.idleBobStrength;
            rb.AddForce(Vector3.up * bob, ForceMode.Acceleration);
        }
    }

    public void GenerateNewConfig(JellyConfig newConfig)
    {
        config = newConfig;
    }
}
