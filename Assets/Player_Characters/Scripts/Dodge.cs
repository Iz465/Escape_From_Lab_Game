using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dodge : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void DodgeLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("Dodging Left!");
        animator.SetBool("DodgeLeft", true);
        StartCoroutine(DodgeLeftTimer(0.3f));

    }


    public void DodgeRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("Dodging Right!");
        animator.SetBool("DodgeRight", true);
    }

    public void DodgeBackwards(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("Dodging Backwards!");
    }

    private IEnumerator DodgeLeftTimer(float time)
    {
        float timer = 0; 
        Vector3 startLoc = transform.root.localPosition;
        Vector3 leftDir = -transform.root.right;
        Vector3 endLoc = startLoc + leftDir * 6f;  // Always moves to left no matter player rotation.

        while (timer < time)
        {
            transform.root.localPosition = Vector3.Lerp(startLoc, endLoc, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }

    }




    private void ResetDodgeLeft()
    {
        animator.SetBool("DodgeLeft", false);
    }

    private void ResetDodgeRight()
    {
        animator.SetBool("DodgeRight", false);
    }

}
