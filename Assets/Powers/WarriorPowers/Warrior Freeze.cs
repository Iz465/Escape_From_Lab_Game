using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine.InputSystem.XR;

public class WarriorFreeze : BasePower
{
    [SerializeField] private GameObject freezeParticle;
    [SerializeField] private Transform particleLocation;
    [SerializeField] private float freezeTime;
    [SerializeField] private Material freezeMaterial;
    [SerializeField] private float freezeCooldown;
    private bool canFreeze = true;
    private Material originalMaterial;

    public void StartFreeze(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canFreeze) return;
        canFreeze = false;
        StartCoroutine(ResetFreeze(freezeCooldown));

        StartAttack(context);
    }

    private void ActivateFreeze()
    {
        
        GameObject freeze = Instantiate(freezeParticle, particleLocation.transform.position, cam.transform.rotation);
        if (!freeze) return;
        Rigidbody body = freeze.GetComponent<Rigidbody>();
        body.AddForce(freeze.transform.forward * 120, ForceMode.Impulse);



    }

    public void FreezeEnemy(navmeshtestscript enemy)
    {

        enemy.canMove = false;

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (!agent) return;
        agent.isStopped = true;

        Animator enemyAnimator = enemy.GetComponent<Animator>();
        if (!enemyAnimator) return;
        enemyAnimator.speed = 0;
       
        originalMaterial = enemy.enemyMesh.material;
        enemy.enemyMesh.material = freezeMaterial;
   
    

        StartCoroutine(Unfreeze(enemy, agent, enemyAnimator, freezeTime));
        
     
    }

    private IEnumerator Unfreeze(navmeshtestscript enemy, NavMeshAgent agent, Animator enemyAnimator, float time)
    {
        yield return new WaitForSeconds(time);
        enemy.canMove = true;
        agent.isStopped = false;
        enemyAnimator.speed = 1;
        enemy.enemyMesh.material = originalMaterial;

    }

    private IEnumerator ResetFreeze(float time)
    {
        yield return new WaitForSeconds(time);
        canFreeze = true;
        Debug.Log("FREEZE RESET");
    }
}
