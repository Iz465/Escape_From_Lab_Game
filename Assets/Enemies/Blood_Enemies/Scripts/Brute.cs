using System.Collections;
using UnityEngine;

public class Brute : BloodEnemy
{
    [SerializeField] LayerMask playerLayer;
    private bool attack;
    private bool charge;
    private bool canChargeDamage;
    private linescript line;


    protected override void Start()
    {
        base.Start();
        attack = true;
        // attackRange = 20;
        attackRange = 7;
        line = GetComponent<linescript>();

    }

    protected override void Update()
    {
        base.Update();

    }

    // random enemy attack chosen.
    protected override void AttackPlayer()
    {

        agent.isStopped = true;

       
        if (attack)
            animator.SetBool("CanAttack", true);
     //   if (charge)
     //   {
           
     //       animator.SetBool("CanCharge", true);
    //        StartCoroutine(Charge(1));
    //    }
          
        canAttack = false;


    }
    // slam attack 
    int number;
    protected override void Attack()
    {

        number++;
 
        Collider[] playerCheck;
        playerCheck = Physics.OverlapSphere(transform.position, 10f, playerLayer);
        
        if (playerCheck.Length > 0)
        {
            player.TakeDamage(50);
        }
        
    }



    // Causes enemy to charge a certain distance in the direction of player.
    private IEnumerator Charge(int time)
    {
        yield return new WaitForSeconds(time);
        number++;
        canChargeDamage = true;
        animator.SetBool("ChargeActivate", true);
        agent.speed = 120;
        agent.acceleration = 120;
        agent.isStopped = false;
        Vector3 chargeLocation = transform.position + transform.forward * 30;
        agent.SetDestination(chargeLocation);

        while (agent.remainingDistance > 0.5f || agent.pathPending)
        {
          Debug.Log($"Distance left {chargeLocation - transform.position}");
          yield return null;
        }



            ResetCharge();
    }

    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        line.toggleCircle = false;
        animator.SetBool("CanAttack", false);
        if (number >= 3)
            StartCoroutine(ResetAttack(2));
        else
            StartCoroutine(ResetAttack(0)); 
    }

    private void ResetCharge()
    {

        agent.speed = 3.5f;
        agent.acceleration = 8;
   
        animator.SetBool("CanCharge", false);
        animator.SetBool("ChargeActivate", false);
        if (number >= 3)
            StartCoroutine(ResetAttack(2));
        else
            StartCoroutine(ResetAttack(1));
    }

    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);

        if (number <= 2 && charge)
        {
            StartCoroutine(Charge(0));
            yield break;
        }

        else if (number <= 2 && attack)
        {
            animator.SetBool("CanAttack", true);
            yield break;
        }

            number = 0;
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
        line.toggleCircle = true;
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
