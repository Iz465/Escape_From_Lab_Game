using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public CharacterController characterController;
    [SerializeField]
    public Player.PlayerStats playerStats;

    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;
    private bool ShiftPressed = false;

    public void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        
    }




    void Update()
    {

        var speed = characterController.velocity.magnitude;

        if (Input.GetKey(KeyCode.W))
            wPressed = true;
        else 
            wPressed = false;
        if (Input.GetKey(KeyCode.S))
            sPressed = true;
        else
            sPressed = false;

        if (Input.GetKey(KeyCode.D))
            dPressed = true;
        else
            dPressed = false;
    
        if (Input.GetKey(KeyCode.A))
            aPressed = true;
        else
            aPressed = false;



        ShiftPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        animator.SetBool("W", wPressed);
        animator.SetBool("A", aPressed);
        animator.SetBool("S", sPressed);
        animator.SetBool("D", dPressed);
        animator.SetBool("Sprint", ShiftPressed);
        

    }
}
