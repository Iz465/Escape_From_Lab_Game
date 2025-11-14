using System;
using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static Unity.Barracuda.TextureAsTensorData;

public class FirePlayerInputs : MonoBehaviour
{
    [SerializeField]
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

    private string animationName = "Attack_1";
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
        StartCoroutine(FireFireBall());
    }
    IEnumerator FireFireBall()
    {
        Debug.Log("Coroutine Started");
       
       
        yield return new WaitUntil(() =>animator.GetBool("AttackReady"),TimeSpan.FromSeconds(15),() => Debug.Log("Fire Ball timed out"));
        GameObject fireball = Instantiate(FireballPrefab, FiringPoint.position, Quaternion.identity);

        fireball.GetComponent<Rigidbody>().AddRelativeForce(FiringPoint.forward * FireBall_Speed, ForceMode.Impulse);
        GameObject.Destroy(fireball, 7F);
        Debug.Log("Coroutine Ended");
    }
    
    public void Attack_2_AreaBlast(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        animator.SetTrigger("Attack_2");
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        animator.SetTrigger("Jump");
    }
    IEnumerator Jumping()
    {
        
        yield return new WaitUntil(() => animator.GetBool("IsJumping"), TimeSpan.FromSeconds(15), () => Debug.Log("Jumping Timed out"));
           


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
