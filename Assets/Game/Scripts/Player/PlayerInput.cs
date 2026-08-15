using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 lookDir {get; private set;}
    public bool pulseInput {get; private set;}

    [Header("cursor")]
    public bool lockCursor = true;


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
        lookDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        pulseInput = Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
