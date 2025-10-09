using System.Collections;
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

    protected bool canPunch = false;

    protected float attacked;

    void Start()
    {
        Application.targetFrameRate = 300;
    }

    protected IEnumerator MeleeAttack(Animator animator)
    {
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

    private void Update()
    {
    }
}
