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

    protected float attacked;

    void Start()
    {
        Application.targetFrameRate = 300;
    }

    protected IEnumerator MeleeAttack(Animator animator)
    {
        animator.SetBool("Idle", false);
        attacked = Time.time + meleeAttackCooldown * Time.timeScale;
        animator.SetBool(meleeAttackAnimation, true);
        yield return new WaitForSeconds(meleeAttackCooldown);
        animator.SetBool("Idle", true);
        animator.SetBool(meleeAttackAnimation, false);
    }

    private void Update()
    {
    }
}
