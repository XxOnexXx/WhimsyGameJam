using UnityEngine;

[CreateAssetMenu(fileName = "JellyConfig", menuName = "Jelly/Movement Config")]
public class JellyConfig : ScriptableObject
{
    
    [Header("Rotation / Steering")]
    [Tooltip("How fast the jellyfish turns to face the mouse-look direction.")]
    public float rotationSpeed = 3f;
 
    [Tooltip("Mouse sensitivity for pitch/yaw look direction.")]
    public float lookSensitivity = 2f;
 
    [Tooltip("Clamp on how far up/down the jellyfish can aim (degrees).")]
    public float pitchClampAngle = 80f;
 
    [Header("Propulsion (Pulse)")]
    [Tooltip("Impulse force applied per pulse, along facing direction.")]
    public float pulseForce = 8f;
 
    [Tooltip("Minimum time between pulses (seconds), prevents spam-click abuse.")]
    public float pulseCooldown = 0.35f;
 
    [Tooltip("Max speed the jellyfish can reach from pulses.")]
    public float maxSpeed = 12f;
 
    [Header("Water Physics")]
    [Tooltip("How quickly velocity decays between pulses (higher = stops faster).")]
    public float waterDrag = 0.6f;
 
    [Tooltip("Upward force to counter gravity, simulates buoyancy. Set close to gravity magnitude for near-neutral buoyancy.")]
    public float buoyancyForce = 9.8f;
 
    [Tooltip("Small constant sink/bob applied when idle, for a 'floating' feel.")]
    public float idleBobStrength = 0.15f;
    public float idleBobSpeed = 1f;
}
