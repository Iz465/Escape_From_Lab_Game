using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlus : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 direction;
    public float acceleration;
    public Vector3 velocity;
    public bool useOtherScript;
    private Animator animator;

    [Header("Edit if character dont have powers that influence speed")]
    private float MoveSpeed;
    public float SprintSpeed = 8f;
    public float NormalSpeed = 5f;
    //fall/just parameters
    float fallSpeed = 0.0f;
    public float fallAcceleration = 0.1f;
    public float jumpStrength = 1.5f;
    private Vector3 Last_position;
    float height;
    bool walking;
    bool Sprinting;
    void Fall()
    {
        Debug.DrawRay(transform.position + new Vector3(0, controller.center.y - 0.1f, 0), Vector3.down * (height / 2));
        if (!Physics.Raycast(transform.position + new Vector3(0, controller.center.y - 0.1f, 0), Vector3.down, height / 2))
        {
            fallSpeed -= fallAcceleration * Time.deltaTime;
            fallSpeed = Mathf.Clamp(fallSpeed, -50, 10);
        }
    }

    void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (Physics.Raycast(transform.position + new Vector3(0, controller.center.y - 0.1f, 0), Vector3.down, height / 2))
            {
                fallSpeed = jumpStrength;
            }
        }
    }
    public void Walk()
    {
        Jump();
        Fall();
        sprint();
        Last_position = controller.transform.position;
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
        if (useOtherScript)
        {
            velocity.y += fallSpeed * Time.deltaTime;
            controller.Move(velocity);
        }
        else
        {
            controller.Move(direction * MoveSpeed * Time.deltaTime);
        }
        var displacement = controller.transform.position - Last_position;
        Debug.Log(displacement.magnitude);
        walking = displacement.magnitude > 0.01 && !Sprinting ? true : false;
        Sprinting = displacement.magnitude > 0.02  ? true : false;
        animator.SetBool("Walking", walking);
        animator.SetBool("Sprinting", Sprinting);
    }
    public void sprint() 
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeed = SprintSpeed;
        }
        else
        {
            MoveSpeed = NormalSpeed;
        }

    }


    private void Start()
    {
        //find objects in the scene to remove the need for public variables
        MoveSpeed = NormalSpeed;
        CharacterController ctrl;
        if (transform.TryGetComponent<CharacterController>(out ctrl))
            controller = ctrl;
        else
            Debug.LogWarning("No controller found in player");
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        height = transform.root.GetComponent<CharacterController>().height;
        Debug.Log( animator.parameters.ToString());
    }

    void Update()
    {
        if (controller != null)
            Walk();


    }
}