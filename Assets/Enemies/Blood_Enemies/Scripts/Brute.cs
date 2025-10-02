using System.Collections;
using UnityEngine;

public class Brute : BloodEnemy
{
    [SerializeField] LayerMask playerLayer;
    public bool toggleCircle;
    private bool attack;
    private bool charge;
    private bool canChargeDamage;

    protected override void Start()
    {
        base.Start();
        charge = true;
        attackRange = 20;
    }

    // random enemy attack chosen.
    protected override void AttackPlayer()
    {
        agent.isStopped = true;
        
        if (attack)
            animator.SetBool("CanAttack", true);
        if (charge)
        {
            Debug.Log("STARTING CHARGE");
            animator.SetBool("CanCharge", true);
            StartCoroutine(Charge(3));
        }
          
        canAttack = false;


    }
    // slam attack 
    protected override void Attack()
    {
       
        
        Debug.Log("SLAM");
        Collider[] playerCheck;
        playerCheck = Physics.OverlapSphere(transform.position, 10f, playerLayer);
        
        if (playerCheck.Length > 0)
        {
            Debug.Log("Player Slammed!");
            player.TakeDamage(50);
        }
        
    }



    // Causes enemy to charge a certain distance in the direction of player.
    private IEnumerator Charge(int time)
    {
        yield return new WaitForSeconds(time);
        canChargeDamage = true;
        animator.SetBool("ChargeActivate", true);
        agent.speed = 120;
        agent.acceleration = 120;
        agent.isStopped = false;
        Vector3 chargeLocation = transform.position + transform.forward * 30;
        agent.SetDestination(chargeLocation);
        Debug.Log(agent.velocity.magnitude);
        while (agent.remainingDistance > 0.5f || agent.pathPending)
            yield return null;


        ResetCharge();
    }

    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        toggleCircle = false;
        animator.SetBool("CanAttack", false);
        StartCoroutine(ResetAttack(3));
    }

    private void ResetCharge()
    {
 
        agent.speed = 3.5f;
        agent.acceleration = 8;
        animator.SetBool("CanCharge", false);
        animator.SetBool("ChargeActivate", false);
        StartCoroutine(ResetAttack(3));
    }

    private IEnumerator ResetAttack(int time)
    {
        yield return new WaitForSeconds(time);

      

        int num = Random.Range(0, 2);
        
        if (num == 0)
        {
            attack = true;
            charge = false;
            attackRange = 10;
        }

        else if (num == 1)
        {
            charge = true;
            attack = false;
            attackRange = 20;
        }

        canAttack = true;
    }


    private void ShowCircle()
    {
        toggleCircle = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canChargeDamage)
        {
            Debug.Log("Cant charge");
            return;
        }
   
        Player playerObject = other.GetComponent<Player>();
        if (!playerObject) return;
        Debug.Log($"Something Hit! {other.gameObject.name}");
        canChargeDamage = false;
        playerObject.TakeDamage(50);
    }


}
