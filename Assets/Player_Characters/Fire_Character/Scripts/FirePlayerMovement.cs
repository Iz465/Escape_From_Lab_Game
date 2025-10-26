using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FirePlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private Animator animator;
    [Header("Character Stats Scriptable Object")]
    public LyraVeyne lyraVeyne;
    public float GracePeriod = 0.2f;
    private float originalStepOffset;
    private float ySpeed;
    public float gravity = -9.81f;
    public float jumpSpeed;
    public float HorazontaljumpSpeed;
    private float? LastOnGroundTime;
    private float? LastJumpTime;
    private bool isJumping;
    private bool isGrounded;
    private bool isFalling;
    private bool isSprinting;
    
    private void Start()
    {
        originalStepOffset = controller.stepOffset;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        float maximumSpeed = lyraVeyne.sprintSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        float InputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
        {
            InputMagnitude /= 2;
        }        
        animator.SetFloat("InputMagnitude", InputMagnitude, 0.05f, Time.deltaTime);
        
       // movementDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;
        
        if (controller.isGrounded)
        {
            LastOnGroundTime = Time.time;
        }
        if (Input.GetButton("Jump"))
        {
            LastJumpTime = Time.time;
        }

        if (Time.time - LastOnGroundTime <= GracePeriod)
        {
            controller.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool(name: "IsGrounded", true);
            isGrounded = true;
            animator.SetBool(name: "IsJumping", false);
            isJumping = false;
            animator.SetBool(name: "IsFalling", false);
            isFalling = false;
            if (Time.time - LastJumpTime <= GracePeriod)
            {
                ySpeed = jumpSpeed;
                animator.SetBool(name: "IsJumping", true);
                isJumping = true;
                LastJumpTime = null;
                LastOnGroundTime = null;
            }
        }
        else
        {
            animator.SetBool(name: "IsGrounded", false);
            isGrounded = false;
            controller.stepOffset = 0;
            if ((isJumping && ySpeed < 0 ) || ySpeed<-2)
            {
                animator.SetBool(name: "IsFalling", true);
                isFalling = true;
            }
        }

        

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, toRotation, lyraVeyne.rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (isGrounded == false)
        {
            Vector3 velocity = movementDirection * InputMagnitude * HorazontaljumpSpeed;
            velocity.y = ySpeed;
            controller.Move(velocity * Time.deltaTime);

        }
    }
    private void OnAnimatorMove()
    {
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime; 
            controller.Move(velocity);
        }
        

       
    }
    //private void OnApplicationFocus(bool focus)
    //{
    //    if (focus)
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        Cursor.visible = false;
    //    }
    //    else
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;
    //    }
    //}












}
