using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    CharacterController controller;
    
    
    public float acceleration;
    public float walkSpeed;
    public float iceSpeed;
    Vector3 velocity;

    void IceWalk(Vector3 dir)
    {
        if (dir.magnitude < 0.1f)
        {
            velocity -= velocity.normalized * acceleration * Time.deltaTime;
        }
        else
        {
            velocity += dir * acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, iceSpeed);
        }
        controller.Move(velocity);
    }

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


        RaycastHit hitObj;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hitObj, transform.lossyScale.y/2 + 1))
        {
            if (hitObj.transform.CompareTag("Ice")) {
                IceWalk(dir);
                return;
            }
        }

        velocity = Vector3.zero;
        controller.Move(dir * Time.deltaTime * walkSpeed);
        
    }
    

    private void Start()
    {
        //find objects in the scene to remove the need for public variables
        
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (controller != null)
            Walk();

        
    }
}
