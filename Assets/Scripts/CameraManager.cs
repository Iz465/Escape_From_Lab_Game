using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform camTarget;
    Transform cam;
    public float minCamDistance, maxCamDistance;
    float camDistance = 5f;
    float lookY;
    void Start()
    {
        camTarget = transform.Find("CameraTarget");
        cam = transform.Find("Main Camera");

        if (!cam)
            cam = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "CameraTarget");

    }

    [SerializeField] private Transform rayLocation;

    float currentCamDistance;
    void LookAround()
    {
        // Get mouse scroll wheel input for zooming the camera
        float axis = Input.GetAxis("Mouse ScrollWheel");
        camDistance -= axis;
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
        Transform rayTransform = rayLocation != null ? rayLocation : transform;

     
        if (Physics.Raycast(rayTransform.position, -camTarget.forward, out hit, camDistance))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Player") &&
                hit.transform.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                camDistance = hit.distance - 0.1f;
            }
        }

        // stops constant camera glitching
        currentCamDistance = Mathf.Lerp(currentCamDistance, camDistance, Time.deltaTime * 10f);


        cam.position = camTarget.position - camTarget.forward * currentCamDistance;

        cam.LookAt(camTarget);
    }

    // Update is called once per frame

    private void Update()
    {
        if (camTarget != null)
            LookAround();
    }
    
        
    



}
