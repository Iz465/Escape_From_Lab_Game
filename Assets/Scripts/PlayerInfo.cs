using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float health, stamina;
    public float lastDamageTime;
    public float maxHealth;

    [SerializeField] protected Animator animator;
    [SerializeField] protected string meleeAttackAnimation;
    [SerializeField] protected float meleeAttackCooldown;
    [SerializeField] protected float attackDuration;

    public Transform spawnPosition;

    protected bool canPunch = false;

    protected float attacked;

    void Start()
    {
        Application.targetFrameRate = 300;
    }

    protected IEnumerator MeleeAttack(Animator animator)
    {
        print("punching");
        animator.speed = 1 / Time.timeScale;
        animator.SetBool("Idle", false);
        attacked = Time.time + meleeAttackCooldown * Time.timeScale;
        animator.SetBool(meleeAttackAnimation, true);
        yield return new WaitForSeconds(attackDuration * Time.timeScale);
        animator.SetBool("Idle", true);
        animator.SetBool(meleeAttackAnimation, false);
        yield return new WaitForSeconds(meleeAttackCooldown * Time.timeScale);
        canPunch = true;
    }

    protected void DamageEnemy()
    {
        print(transform.name);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Transform enemyTransform = enemy.transform;
            if ((enemyTransform.position - transform.position).magnitude > 4) continue;
            Vector3 enemyDirection = (enemyTransform.position - transform.position).normalized;

            if (Vector3.Dot(enemyDirection, transform.forward) < 0.5f) continue;
            enemy.GetComponent<Soldier>().GotHit();
        }
    }
}
