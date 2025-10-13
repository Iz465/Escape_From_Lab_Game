using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MeleeZombie : navmeshtestscript
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private LayerMask playerLayer;
    private int number;
    private bool checkHitbox;
    private bool canHit;
    

    protected override void Start()
    {
        base.Start();
        number = 0;
        checkHitbox = false;
        canHit = false;

    }

    protected override void Update()
    {
        base.Update();

        if (checkHitbox && canHit)
        {
            bool checkPlayer = Physics.Linecast(point1.transform.position, point2.transform.position, playerLayer);
            
            if (checkPlayer)
            {
                player.TakeDamage(20);
                canHit = false;
            }
                
         
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point1.position, point2.position);
        
    }
    



    protected override void AttackPlayer()
    {
        base.AttackPlayer();
     //   canRotate = false;
    }


    private void ResetAttack()
    {
        number++;

        if (number < 3) return;
        StartCoroutine(CanChase(1.5f));
        number = 0;
        canRotate = true;
    }

    private IEnumerator CanChase(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void EnableHit()
    {
        canHit = true;
        checkHitbox = true;
    }

    private void DisableHit()
    {
        canHit = false;
        checkHitbox = false;
    }

  

   


}
