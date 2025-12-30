using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dodge : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float cooldown;
    [SerializeField] private float dodgeDistance;
    private bool canDodge = true;
    private Player player;
    [SerializeField] private AudioClip footstepSound;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
       
    }
    public void DodgeLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canDodge) return;
        StartCoroutine(DodgeLeftTimer(0.1f));
        canDodge = false;
        StartCoroutine(ResetDodge(cooldown));

    }


    public void DodgeRight(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canDodge) return;
        StartCoroutine(DodgeRightTimer(0.1f));
        canDodge = false;
        StartCoroutine(ResetDodge(cooldown));
    }

    public void DodgeBackwards(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canDodge) return;
        StartCoroutine(DodgeBackTimer(0.1f));
        canDodge = false;
        StartCoroutine(ResetDodge(cooldown));
    }

    private IEnumerator DodgeLeftTimer(float time)
    {
        float timer = 0; 
        Vector3 startLoc = transform.root.localPosition;
        Vector3 left = -transform.root.right;
        Vector3 endLoc = startLoc + left * dodgeDistance;  // Always moves to left no matter player rotation. dodgeDistance

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
        Vector3 endLoc = startLoc + right * dodgeDistance;  // Always moves to left no matter player rotation.

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
        Vector3 endLoc = startLoc + back * dodgeDistance;  // Always moves to left no matter player rotation. 

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


    private IEnumerator ResetDodge(float time)
    {
        yield return new WaitForSeconds(time);
        canDodge = true;
    }

    private void PlayFootstepSound()
    {
        player.audioSource.PlayOneShot(footstepSound);
    }
}
