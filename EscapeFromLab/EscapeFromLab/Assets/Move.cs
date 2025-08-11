using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    CharacterController controller;
    Transform camTarget;
    Camera cam;
    
    void Walk()
    {
        Vector3 dir = new();
        if (Input.GetKey(KeyCode.W))
            dir += Vector3.forward;
        else if (Input.GetKey(KeyCode.S))
            dir += Vector3.back;
        

        if (Input.GetKey(KeyCode.D))
            dir += Vector3.right;
        else if (Input.GetKey(KeyCode.A))
            dir += Vector3.left;
        
        controller.Move(dir);

    }
    float lastMouse;
    void LookAround()
    {
        Vector2 mouseDir = Input.mousePositionDelta;
        transform.Rotate(0f, mouseDir.x, 0f);
        camTarget.Rotate(mouseDir.y, 0f, 0f);

        if (mouseDir.y > 0 && camTarget.rotation.x < .4f)
            camTarget.Rotate(mouseDir.y, 0, 0);

        if (mouseDir.y < 0 && camTarget.rotation.x > -.02f)
            camTarget.Rotate(mouseDir.y, 0, 0);

        if (mouseDir.y != 0)
            lastMouse = mouseDir.y;
            
        print(lastMouse);
        print(camTarget.rotation.x);
        /*
        if (camTarget.rotation.x < -.4f || camTarget.rotation.x > .4f)
        {
            print("looking too low");
            // camTarget.Rotate(-camTarget.rotation.x, 0, 0);
        }

        if (camTarget.rotation.x > .4f)
        {
            print("looking too high");
        //    camTarget.Rotate(-camTarget.rotation.x, 0, 0);
        }
        */
        cam.transform.position = camTarget.position + camTarget.forward * -3;
        cam.transform.LookAt(camTarget);
    }

    private void Start()
    {
        camTarget = transform.Find("CameraTarget");
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        if(controller != null)
            Walk();

        if(cam != null)
            LookAround();
    }
}
