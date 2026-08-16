using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 lookDir {get; private set;}
    public bool pulseInput {get; private set;}

    [Header("cursor")]
    public bool lockCursor = true;

    [Range(0f, 0.3f)] public float lookSmoothTime = 0.08f;

    Vector2 rawlookDelta;
    Vector2 smoothedLookVelocity;


    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void Update()
    {   
        rawlookDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        lookDir = Vector2.SmoothDamp(lookDir, rawlookDelta, ref smoothedLookVelocity, lookSmoothTime);
        
        pulseInput = Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
