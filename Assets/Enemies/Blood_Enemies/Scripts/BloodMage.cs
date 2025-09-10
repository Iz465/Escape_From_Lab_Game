using System.Collections;
using UnityEngine;

public class BloodMage : BloodEnemy
{
    [SerializeField]
    private GameObject power;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform aimLoc;

    protected Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("CanFind", true);
            Debug.Log("Found The Animator");
        }

        else
            Debug.Log("Not available");

    }

    public override void Attack()
    {
      
        Debug.Log("Attack Func Called");
        // GameObject powerInstance = Instantiate(power, transform.position, transform.rotation);
        GameObject powerInstance = Instantiate(power, aimLoc.position, transform.rotation);
        if (!powerInstance) return;
        Rigidbody rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb) return;
        Vector3 aimDir = player.position - aimLoc.position;
        rb.AddForce(aimDir, ForceMode.Impulse);
        StartCoroutine(resetAnim(2));

    }

    private IEnumerator resetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("CanAttack", false);
        if (direction.magnitude > attackRange)
           animator.SetBool("CanFind", true);
        else
            animator.SetBool("CanFind", false);

    }

    private void Update()
    {
        direction = player.position - transform.position;

        if (direction.magnitude > attackRange)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            if (controller.enabled == true)
                controller.Move(direction.normalized * walkSpeed * Time.deltaTime);

        }

        else if (canAttack)
        {
            animator.SetBool("CanAttack", true);
            StartCoroutine(ResetAttack(cooldown));
            canAttack = false;
        }

        animator.SetFloat("PlayerDistance", direction.magnitude);
    }

}
