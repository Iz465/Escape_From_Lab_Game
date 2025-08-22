using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    CharacterController controller;
    public Vector3 direction;
    public float acceleration;
    public Vector3 velocity;
    public bool useOtherScript;

    [Header("Edit if character dont have powers that influence speed")]
    public float walkSpeed;

    public void Walk()
    {
        direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
            direction += transform.forward;
        if (Input.GetKey(KeyCode.S))
            direction -= transform.forward;

        if (Input.GetKey(KeyCode.D))
            direction += transform.right;
        if (Input.GetKey(KeyCode.A))
            direction -= transform.right;

        if (useOtherScript)
            controller.Move(velocity);
        else
            controller.Move(direction * walkSpeed * Time.deltaTime);
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
