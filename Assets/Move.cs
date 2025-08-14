using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    CharacterController controller;
    Transform camTarget;
    //Camera cam;

    float lookY;
    public float walkSpeed;

    void Walk()
    {
        Vector3 dir = new();
        if (Input.GetKey(KeyCode.W))
            dir += transform.forward;
        else if (Input.GetKey(KeyCode.S))
            dir -= transform.forward;


        if (Input.GetKey(KeyCode.D))
            dir += transform.right;
        else if (Input.GetKey(KeyCode.A))
            dir -= transform.right;

        controller.Move(dir * Time.deltaTime * walkSpeed);

    }
    float lastMouse;
    void LookAround()
    {
        Vector2 mouseDir = Input.mousePositionDelta;
        float lookX = mouseDir.x;
        lookY -= mouseDir.y;

        lookY = Mathf.Clamp(lookY, -45, 45);

        transform.Rotate(0, lookX, 0f);
        camTarget.localRotation = Quaternion.Euler(lookY, 0, 0f);
    }

    private void Start()
    {
        camTarget = transform.Find("Head");
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (controller != null)
            Walk();

        if (camTarget != null)
            LookAround();
    }
}
