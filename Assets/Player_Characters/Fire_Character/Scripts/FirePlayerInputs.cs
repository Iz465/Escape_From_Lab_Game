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

        while (true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
            if (clip.name == animationName)
            {
                float normalizedTime = stateInfo.normalizedTime % 1; // Get normalized time (0 to 1)
                int currentFrame = Mathf.FloorToInt(normalizedTime * clip.frameRate * clip.length);

                Debug.Log($"Current Frame: {currentFrame}");
            }
            
        }

        
        GameObject fireball = Instantiate(FireballPrefab, FiringPoint.position, Quaternion.identity);
        Vector3 movementDirection = controller.transform.position += FiringPoint.position;
        
        movementDirection.y = FiringPoint.position.y;
        Vector3 force = new(movementDirection.x * 10f, movementDirection.y, movementDirection.z * 10f);
        fireball.GetComponent<Rigidbody>().AddRelativeForce(force);
        GameObject.Destroy(fireball,7F);
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
