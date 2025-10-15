using System.Collections;
using UnityEngine;

public class EvilKnight : navmeshtestscript
{
    protected override void AttackPlayer()
    {
        canAttack = false;
        animator.SetTrigger("Slash");
    }

    private void ResetAnim()
    {
        StartCoroutine(CanAttack(2));
    }

    private IEnumerator CanAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }
}
