using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BloodZombie : MonoBehaviour
{
    private NavMeshAgent agent;
    private Player player;
    private Animator animator;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Transform explosionLocation;
    [SerializeField] private LayerMask playerLayer;

    [System.Serializable]
    public struct CorpseParts
    {
        public GameObject head;
        public GameObject torso;
        public GameObject leftHand;
        public GameObject rightHand;
        public GameObject legs;

    }

    public CorpseParts corpseParts;
    private linescript line;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();

        line = GetComponent<linescript>();
       
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= 80)
            agent.SetDestination(player.transform.position);
        if (distanceToPlayer < 2)
        {
            agent.isStopped = true;
            animator.SetBool("CanAttack", true);
            line.toggleCircle = true;
        }
        

    }

    private void Explode()
    {
  

        if (corpseParts.head)
            MakeRagdoll(corpseParts.head, 2);
        if (corpseParts.legs)
            MakeRagdoll(corpseParts.legs, 0.5f);
        if (corpseParts.rightHand)
            MakeRagdoll(corpseParts.rightHand, 1.5f);
        if (corpseParts.leftHand)
            MakeRagdoll(corpseParts.leftHand, 1.5f);
        if (corpseParts.torso)
            MakeRagdoll(corpseParts.torso, 1.5f);
      
        Collider[] playerCheck = Physics.OverlapSphere(transform.position, 4, playerLayer);
        if (playerCheck.Length > 0)
            player.TakeDamage(50);

        Destroy(gameObject);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4);
    }

    private void MakeRagdoll(GameObject bodypart, float height)
    {
        if (bodypart)
        {

            GameObject ragdoll = Instantiate(bodypart, transform.position + new Vector3(0, height, 0), Quaternion.identity);
            Vector3 hitDirection = (ragdoll.transform.position - player.transform.position).normalized;
            ragdoll.transform.rotation = Quaternion.LookRotation(hitDirection) * Quaternion.Euler(90, 0, 0);


            Rigidbody rigid = ragdoll.GetComponent<Rigidbody>();
            if (rigid)
            {

                rigid.AddForce(hitDirection * 10, ForceMode.Impulse);
                rigid.AddTorque(Random.insideUnitSphere * 1f, ForceMode.Impulse);

            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BasePower power = collision.gameObject.GetComponent<BasePower>();
        if (!power) return;
        Debug.Log("Hit Zombie!!");
        Explode();
    }
}
