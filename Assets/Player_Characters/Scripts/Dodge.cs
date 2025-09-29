using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dodge : MonoBehaviour
{
    private Animator animator;
    [SerializeField] int cooldown;
    private bool canDodge;
    private void Start()
    {
        animator = GetComponent<Animator>();
        canDodge = true;
    }
    public void DodgeLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canDodge) return;
        animator.SetBool("DodgeLeft", true);
        StartCoroutine(DodgeLeftTimer(0.3f));
        canDodge = false;
        StartCoroutine(ResetDodge(cooldown));

    }


    public void DodgeRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canDodge) return;
        animator.SetBool("DodgeRight", true);
        StartCoroutine(DodgeRightTimer(0.3f));
        canDodge = false;
        StartCoroutine(ResetDodge(cooldown));
    }

    public void DodgeBackwards(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canDodge) return;
        animator.SetBool("DodgeBack", true);
        StartCoroutine(DodgeBackTimer(0.3f));
        canDodge = false;
        StartCoroutine(ResetDodge(cooldown));
    }

    private IEnumerator DodgeLeftTimer(float time)
    {
        float timer = 0; 
        Vector3 startLoc = transform.root.localPosition;
        Vector3 left = -transform.root.right;
        Vector3 endLoc = startLoc + left * 6f;  // Always moves to left no matter player rotation.

        while (timer < time)
        {
            transform.root.localPosition = Vector3.Lerp(startLoc, endLoc, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator DodgeRightTimer(float time)
    {
        float timer = 0;
        Vector3 startLoc = transform.root.localPosition;
        Vector3 right = transform.root.right;
        Vector3 endLoc = startLoc + right * 6f;  // Always moves to left no matter player rotation.

        while (timer < time)
        {
            transform.root.localPosition = Vector3.Lerp(startLoc, endLoc, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator DodgeBackTimer(float time)
    {
        float timer = 0;
        Vector3 startLoc = transform.root.localPosition;
        Vector3 back = -transform.root.forward;
        Vector3 endLoc = startLoc + back * 6f;  // Always moves to left no matter player rotation.

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

    private void ResetDodgeBack()
    {
        animator.SetBool("DodgeBack", false);
    }

    private IEnumerator ResetDodge(int time)
    {
        yield return new WaitForSeconds(time);
        canDodge = true;
    }

}
