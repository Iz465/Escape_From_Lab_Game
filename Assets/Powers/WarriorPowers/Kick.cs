using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.MLAgents;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;


using static ArcSwing;
using System.Collections;

public class Kick : BasePower
{
    [SerializeField] private Collider kickCollider;
    [SerializeField] private Animator kickAnimator;




    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        kickAnimator.SetTrigger("Kick");

    }



    [SerializeField] private LayerMask enemyLayer;
    private void ActivateKick()
    {
        float kickLength = 5f;
        Vector3 boxCenter = kickCollider.bounds.center + player.transform.forward * (kickLength / 2f);
        Vector3 boxHalfExtent = new Vector3(5f, 5f, kickLength / 2f);

        Collider[] enemyAmount = Physics.OverlapBox(boxCenter, boxHalfExtent, player.transform.rotation, enemyLayer);
        Debug.Log("ACTIVATE KICK FUNCTION");

        if (enemyAmount.Length == 0) return;
        HashSet<GameObject> enemiesKicked = new HashSet<GameObject>();
        foreach (Collider enemyCollider in enemyAmount)
            enemiesKicked.Add(enemyCollider.transform.root.gameObject);
         
        foreach (GameObject enemy in enemiesKicked)
        {

            navmeshtestscript enemy1 = enemy.GetComponent<navmeshtestscript>();  // disables agent movement to stop its interferrance with the kick pushing the enemy.
            if (!enemy1) return;
            NavMeshAgent agent = enemy1.GetComponent<NavMeshAgent>();
            if (!agent) return;
            Vector3 endDirection = player.transform.forward;
            agent.Move(new Vector3(endDirection.x, 0, endDirection.z) * 10);
            enemy1.TakeDamage(0);
            agent.enabled = false;
            StartCoroutine(EnableMovement(agent, 2));


        }
    

    }

    private IEnumerator EnableMovement(NavMeshAgent agent, float time)
    {
        yield return new WaitForSeconds(time);
        agent.enabled = true;
        
    }


    private void CombatStateEntered()
    {

        animator.SetBool("NotAttacking", false);

    }

    private void NonCombatStateEntered()
    {
 
        animator.SetBool("NotAttacking", true);
        animator.ResetTrigger(stats.powerName);
    }


    private void DeactivateKick()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;

        float kickLength = 5f;
        float kickWidth = 1f;
        float kickHeight = 1f;

        // Center of the box in front of the player
        Vector3 boxCenter = kickCollider.bounds.center + player.transform.forward * (kickLength / 2f);

        // Full size of the box
        Vector3 boxSize = new Vector3(kickWidth, kickHeight, kickLength);

        // Rotate the gizmo to match player rotation
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, player.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }



}
