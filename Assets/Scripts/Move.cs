using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 direction;
    public float acceleration;
    public Vector3 velocity;
    public bool useOtherScript;

    [Header("Edit if character dont have powers that influence speed")]
    public float walkSpeed;

    //fall/just parameters
    float fallSpeed = 0;
    [Header("Multiplies based on walk speed")]
    public float runSpeed;
    public float fallAcceleration = 5;
    public float jumpStrength = 1.5f;

    [SerializeField] Transform torso;
    float height;

    public LayerMask checkLayer;
    void Fall()
    {
        //Debug.DrawRay(transform.position + new Vector3(0,controller.center.y-0.1f,0), Vector3.down * (controller.height / 2));
        RaycastHit hit;
        if(!Physics.Raycast(torso.position + new Vector3(0, controller.center.y-0.1f, 0), Vector3.down, out hit, controller.height / 2, checkLayer))
        {
            //print("falling");
            fallSpeed -= fallAcceleration * Time.deltaTime;
            fallSpeed = Mathf.Clamp(fallSpeed, -50, 10);
        }
        //print(hit.transform);

        //head bumps
        //Debug.DrawRay(torso.position + new Vector3(0, controller.center.y + 0.2f, 0), Vector3.up * controller.height / 2, Color.blue, 0.1f, false);
        //print("drew ray");
        if (Physics.Raycast(torso.position + new Vector3(0,controller.center.y+0.2f,0), Vector3.up, out hit, controller.height/2, checkLayer))
        {
            //print(hit.point);
            if (fallSpeed > 0)
                fallSpeed = 0;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Physics.Raycast(transform.position + new Vector3(0, controller.center.y - 0.1f, 0), Vector3.down, height / 2, checkLayer))
            {
                fallSpeed = jumpStrength;
            }
        }
    }
    public void Walk()
    {
        Jump();
        Fall();

        direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
            direction += transform.forward;
        if (Input.GetKey(KeyCode.S))
            direction -= transform.forward;

        if (Input.GetKey(KeyCode.D))
            direction += transform.right;
        if (Input.GetKey(KeyCode.A))
            direction -= transform.right;

        direction.y += fallSpeed;

        if (!TryGetComponent(out Speed c))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                direction.x *= runSpeed;
                direction.z *= runSpeed;
            }
        }

        if (useOtherScript)
        {
            velocity.y += fallSpeed * Time.deltaTime;
            controller.Move(velocity);
        }
        else
        {
            controller.Move(direction * walkSpeed * Time.deltaTime);
        }
    }
    

    private void Start()
    {
        //find objects in the scene to remove the need for public variables

        CharacterController ctrl;
        if (transform.TryGetComponent<CharacterController>(out ctrl))
            controller = ctrl;
        else
            Debug.LogWarning("No controller found in player");
        Cursor.lockState = CursorLockMode.Locked;

        height = transform.GetComponent<CharacterController>().height;
    }

    void Update()
    {
        if (controller != null)
            Walk();

        
    }
}
