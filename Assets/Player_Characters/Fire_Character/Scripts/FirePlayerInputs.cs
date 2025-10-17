using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirePlayerInputs : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    float jumpHeight = 0.1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void OnAnimatorMove()
    {
        animator.ApplyBuiltinRootMotion();
    }
    public void Sprint(InputAction.CallbackContext context)
    {

        if (!context.started) return;
        animator.SetTrigger("Sprinting");
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        animator.SetTrigger("Jump");
        StartCoroutine(jumpTimer(jumpHeight));
    }
    private IEnumerator jumpTimer(float time)
    {
        float timer = 0;
        Vector3 StartLoc = transform.root.localPosition;
        Vector3 up = transform.root.up;
        Vector3 EndLoc = StartLoc + up * 3f;
        while (timer < time)
        {
            transform.root.localPosition = Vector3.Lerp(StartLoc, EndLoc, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
