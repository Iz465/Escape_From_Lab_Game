using UnityEngine;
using UnityEngine.AI;

public class ChaseAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected PlayerInfo playerInfo;
    public Transform plr;

    [SerializeField] protected float attackRange;
    protected bool canAttack;

    [SerializeField] protected GameObject greenHealthImage;

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;

    [SerializeField] protected bool canHeal;
    [SerializeField] protected float healSpeed;
    protected float gotHit;
    [SerializeField] protected float healDelay;

    protected void Chase(Transform obj)
    {
        bool set = agent.SetDestination(obj.position);
        print("set desination: "+set.ToString());
    }

    protected bool DetectObj(Transform obj)
    {
        //only return true if obj is found
        RaycastHit hit;
        if(Physics.Raycast(transform.position, (obj.position - transform.position), out hit))
        {
            if (hit.transform == obj)
                return true;
            return false;
        }
        return false;
    }

    protected bool IsInFov(Transform obj)
    {
        Vector3 direction = obj.position - transform.position;

        float fov = Vector3.Dot(transform.forward, direction.normalized);
        
        return fov > 0.5f;
    }

    protected bool CanAttack(Transform player)
    {
        if((transform.position - player.position).magnitude < attackRange && canAttack)
        {
            canAttack = false;
            return true;
        }
        return false;
    }

    protected void ShowHealth()
    {
        greenHealthImage.GetComponent<RectTransform>().localScale = new Vector3(health / maxHealth, 1, 1);

    }

    protected void Heal()
    {
        if (!canHeal) return;
        if (health >= maxHealth) return;

        if(Time.time > gotHit + healDelay)
        {
            health += healSpeed * Time.deltaTime * Time.timeScale;
            if(health > maxHealth)
                health = maxHealth;
        }
    }
}
