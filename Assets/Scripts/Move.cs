using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    CharacterController controller;
    Transform camTarget;
    Transform cam;
    public float minCamDistance, maxCamDistance;
    float camDistance = 5f;
    float lookY;
    public float walkSpeed;

    void Walk()
    {
        Vector3 dir = new();
        // Get input for movement forward/backward direction
        if (Input.GetKey(KeyCode.W))
            dir += transform.forward;
        else if (Input.GetKey(KeyCode.S))
            dir -= transform.forward;

        // Get input for movement left/right direction
        if (Input.GetKey(KeyCode.D))
            dir += transform.right;
        else if (Input.GetKey(KeyCode.A))
            dir -= transform.right;

        controller.Move(dir * Time.deltaTime * walkSpeed);

    }
    void LookAround()
    {
        // Get mouse scroll wheel input for zooming the camera
        float axis = Input.GetAxis("Mouse ScrollWheel");
        camDistance += axis;
        camDistance = Mathf.Clamp(camDistance, minCamDistance, maxCamDistance);

        // Get mouse movement for looking around
        Vector2 mouseDir = Input.mousePositionDelta;
        float lookX = mouseDir.x;
        lookY -= mouseDir.y;

        // Clamp vertical look angle to prevent flipping
        lookY = Mathf.Clamp(lookY, -45, 45);

        // Rotate the player and camera based on mouse movement
        transform.Rotate(0, lookX, 0f);
        camTarget.localRotation = Quaternion.Euler(lookY, 0, 0f);

        // Adjust camera position based on raycast to avoid clipping through objects
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -camTarget.forward, out hit, camDistance))
            cam.position = camTarget.position - camTarget.forward * hit.distance;
        else
            cam.position = camTarget.position - camTarget.forward * camDistance;

        
        cam.LookAt(camTarget);
    }

    private void Start()
    {
        //find objects in the scene to remove the need for public variables
        camTarget = transform.Find("CameraTarget");
        cam = transform.Find("Main Camera");
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
