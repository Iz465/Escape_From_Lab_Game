using System;
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
    [Header("Character Stats Scriptable Object")]
    public LyraVeyne lyraVeyne;


    [Header("Edit if character dont have powers that influence speed")]
    private float MoveSpeed;
    public float SprintSpeed;
    public int StaminaDrain_PerSec = 10;
    public int StaminaGain_PerSec = 10;

    public int StaminaCostPerJump = 15;
    public float SprintStaminaThreshold = 20f;
    public float NormalSpeed;
    //fall/just parameters
    float fallSpeed = 0.0f;
    public float fallAcceleration = 0.1f;
    public float jumpStrength = 1.5f;
    private bool isGrounded;
    private bool isJumping;
    private bool IsFalling;
    private Vector3 Last_position;
    float Stamina;
    float height;
    bool walking;
    bool Sprinting;
    // And in Start(), initialize SprintSpeed after lyraVeyne is available:
    private void Start()
    {
        //find objects in the scene to remove the need for public variables
        if (lyraVeyne != null)
        {

            Stamina = Mathf.Clamp(lyraVeyne.Stamina,0f,100f) ;
            NormalSpeed = lyraVeyne.walkSpeed;
            SprintSpeed = lyraVeyne.sprintSpeed;
        }
        else
        {
            SprintSpeed = 8f; // fallback value
            NormalSpeed = 5f; // fallback value
        }
        MoveSpeed = NormalSpeed;
        CharacterController ctrl;
        if (transform.TryGetComponent<CharacterController>(out ctrl))
            controller = ctrl;
        else
            Debug.LogWarning("No controller found in player");
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        height = transform.root.GetComponent<CharacterController>().height;
        Debug.Log(animator.parameters.ToString());

        
    }
    void Fall()
    {
        Debug.DrawRay(transform.position + new Vector3(0, controller.center.y - 0.1f, 0), Vector3.down * (height / 2));
        if (!Physics.Raycast(transform.position + new Vector3(0, controller.center.y - 0.1f, 0), Vector3.down, height / 2))
        {
            fallSpeed -= fallAcceleration * Time.deltaTime;
            fallSpeed = Mathf.Clamp(fallSpeed, -50, 10);
        }
    }
    public bool CanJump()
    {
        return Stamina >= StaminaCostPerJump;
    }
    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            animator.SetBool("IsJumping", true);
            isJumping = true;
            Stamina -= StaminaCostPerJump;
            if (controller.isGrounded)
            {
                fallSpeed = jumpStrength;
            }
            else { 
                animator.SetFloat("Jump 0", 30);
            }
        }
        if (controller.isGrounded)
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("IsFalling", false);
            IsFalling = false;
            fallSpeed = 0;
            animator.SetFloat("Jump 0", 0);
        }
    }
    public void Walk()
    {
        Jump();
        Fall();
        sprint();
        StaminaDrain();
        Last_position = controller.transform.position;
        direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
            { direction += transform.forward; }
      
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
       // walking = displacement.magnitude > 0.001? true : false;
        
        
    }
    public void sprint() 
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Stamina < SprintStaminaThreshold && !Sprinting)
            { 
               Sprinting = false;
            } 
            else
            {
                Sprinting = true;
                
            }
        }
        else
        {
            Sprinting = false;
        }
        animator.SetBool("Sprinting", Sprinting);
    }
    public void StaminaDrain() 
    { 
        if (Sprinting)
        {
            Stamina -= (Stamina >= 0f) ? StaminaDrain_PerSec * Time.deltaTime : 0f; 
            if (Stamina <= 0)
            {
                Stamina = 0;
                MoveSpeed = NormalSpeed;
                Sprinting = false;
            }
            else
            {
                MoveSpeed = SprintSpeed;
            }
            
        }
        else 
        {
            Stamina +=  (Stamina <=  100) ? StaminaGain_PerSec * Time.deltaTime : 0f;
            MoveSpeed = NormalSpeed;
        }
        lyraVeyne.Stamina = Stamina;
        lyraVeyne.DisplayStamina = $"{Stamina:F0}";
    }

   

    void Update()
    {
        if (controller != null)
            Walk();
        if (controller.velocity.magnitude > 0.1f) // Adjust threshold as needed
        {
            walking = true;
            Debug.Log("Character is moving");
        }
        else
        {
            walking = false;
            Debug.Log("Character is stationary");
        }
        animator.SetBool("Walking", walking);
        //  Debug.Log("Stamina: " + Stamina);
    }
}