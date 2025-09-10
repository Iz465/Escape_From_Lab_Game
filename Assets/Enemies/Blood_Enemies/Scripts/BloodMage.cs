using System.Collections;
using UnityEngine;

public class BloodMage : BloodEnemy
{
    [SerializeField]
    private GameObject power;
    [SerializeField]
    private float speed;

    protected Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("CanFind", true);
            Debug.Log("Found the animatpr");
        }

        else
            Debug.Log("Not available");
    }

    protected override void Attack()
    {
        animator.SetBool("CanAttack", true);
        GameObject powerInstance = Instantiate(power, transform.position, transform.rotation);
        if (!powerInstance) return;
        Rigidbody rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb) return;
        rb.AddForce(direction, ForceMode.Impulse);

    }



}
