using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class FirePlayerInputs : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Transform FiringPoint;
    [SerializeField]
    private GameObject FireballPrefab;
    [SerializeField]
    [DefaultValue(175f)]
    [Range(0f,350f)]
    private float FireBall_Speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Instantiate(FireballPrefab, FiringPoint.position, Quaternion.identity);
    }
    public void Attack_1_Fireball(InputAction.CallbackContext context)
    {
        Debug.Log("Attack 1 Input Received");
        if (!context.performed) return;
        animator.SetTrigger("Attack_1");
        GameObject fireball = Instantiate(FireballPrefab, FiringPoint.position, Quaternion.identity);
        Vector3 movementDirection = controller.transform.position += FiringPoint.position;
        movementDirection.y = FiringPoint.position.y;
        Vector3 force = new(movementDirection.x * 10f, movementDirection.y, movementDirection.z * 10f);
        fireball.GetComponent<Rigidbody>().AddRelativeForce(force);
    }
    public void Attack_2_AreaBlast(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        animator.SetTrigger("Attack_2");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
