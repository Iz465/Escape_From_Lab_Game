using System.Collections;
using UnityEngine;

public class Brute : BloodEnemy
{
    [SerializeField] LayerMask playerLayer;
    public bool toggleCircle;
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

    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        toggleCircle = false;
        animator.SetBool("CanAttack", false);
        StartCoroutine(ResetAttack(3));
    }

    private IEnumerator ResetAttack(int time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void ShowCircle()
    {
        toggleCircle = true;
    }


}
