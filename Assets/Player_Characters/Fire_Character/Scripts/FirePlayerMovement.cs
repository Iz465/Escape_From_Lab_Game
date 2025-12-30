using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FirePlayerMovement : MonoBehaviour
{
    [Header("\n\nReferences\n\n")]
        [SerializeField]
        private CharacterController controller;
        private Animator animator;
        [SerializeField]
        private Transform camTransform;
    [Header("\n\nCharacter Stats Scriptable Object \n\n")]
        [SerializeField]
        private LyraVeyne lyraVeyne;
        [SerializeField]
        private float GracePeriod = 0.2f;
        /*Movement Vars*/
        private float originalStepOffset;
        private float ySpeed;
    [Header("\n\nJump Adjustments\n\n")]
        [SerializeField]
        private float gravityMult;
        [SerializeField]
        private float jumpHeight = 2f;
        [SerializeField]
        private float HorizontalSpeed = 3f;
        private float? LastOnGroundTime;
        private float? LastJumpTime;
    [Header("\n\nStamina Costs\n\n")]
        [SerializeField]
        private float StaminaDrain_Sprint = 10;
        [SerializeField]
        private float StaminaDrain_Jump = 10;
        [SerializeField]
        private float StaminaRegen_Moving = 1.5f;
        [SerializeField]
        private float StaminaRegen_Idle = 3f;

    /*Movement Animators*/
    private bool isJumping;
    private bool isGrounded;
    private float InputMagnitude;

    private void Start()
    {
        originalStepOffset = controller.stepOffset;
        animator = GetComponent<Animator>();
        lyraVeyne.ResetStamina();
    }
    private void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        InputMagnitude = Mathf.Clamp01(movementDirection.magnitude);



        if (Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
        {
            InputMagnitude /= 3;
        }
        else if (Input.GetButton("Sprint"))
        {
            lyraVeyne.ReduceStamina(StaminaDrain_Sprint * Time.deltaTime);
            if (lyraVeyne.Stamina <= 0) 
            {
                InputMagnitude /= 3;
            }
        }
        else
        {
            InputMagnitude = (InputMagnitude /= 3)*2;
        }
        
        animator.SetFloat("InputMagnitude", InputMagnitude, 0.05f, Time.deltaTime);
        
        movementDirection = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        float Gravity = Physics.gravity.y * gravityMult;
        

        if (isJumping && ySpeed >0 && Input.GetButton("Jump") == false)
        {
            Gravity *= 2 ;
        }
        ySpeed += Gravity * Time.deltaTime;

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
            
            if (Time.time - LastJumpTime <= GracePeriod && lyraVeyne.Stamina >= 15f)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * Gravity);
                animator.SetBool(name: "IsJumping", true);
                lyraVeyne.ReduceStamina(StaminaDrain_Jump);
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
            if ((isJumping && ySpeed < 0 ) || ySpeed < -2)
            {
                animator.SetBool(name: "IsFalling", true);
                
            }
        }

        

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            lyraVeyne.IncreaseStamina(StaminaRegen_Moving * Time.deltaTime);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, toRotation, lyraVeyne.rotationSpeed * Time.deltaTime);
        }
        else
        {
            lyraVeyne.IncreaseStamina(StaminaRegen_Idle * Time.deltaTime);
            animator.SetBool("IsMoving", false);
        }

        if (isGrounded == false)
        {
            Vector3 velocity = HorizontalSpeed * InputMagnitude * movementDirection;
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
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
           // Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
           // Cursor.visible = true;
        }
    }












}
