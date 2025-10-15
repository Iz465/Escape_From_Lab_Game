using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MeleeZombie : navmeshtestscript
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point1Second;
    [SerializeField] private Transform point2Second;
    [SerializeField] private Transform point1Third;
    [SerializeField] private Transform point2Third;
    [SerializeField] private LayerMask playerLayer;
    private bool checkHitbox;
    private bool canHit;
    private int randomNumber;

    protected override void Start()
    {
        base.Start();
  
        checkHitbox = false;
        canHit = false;
        rotateSpeed = 100;

    }

    protected override void Update()
    {
        base.Update();

       

        if (checkHitbox && canHit)
        {
         
            bool checkPlayer = Physics.Linecast(point1.transform.position, point2.transform.position, playerLayer);
            bool checkPlayer1 = Physics.Linecast(point1Second.transform.position, point2Second.transform.position, playerLayer);
            bool checkPlayer2 = Physics.Linecast(point1Third.transform.position, point2Third.transform.position, playerLayer);
            if (checkPlayer || checkPlayer1 || checkPlayer2)
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
        Gizmos.DrawLine(point1Second.position, point2Second.position);
        Gizmos.DrawLine(point1Third.position, point2Third.position);

    }
    



    protected override void AttackPlayer()
    {

        randomNumber = Random.Range(0, 2);
        canAttack = false;
        canRotate = true;
     
        if (randomNumber == 0)
        {
            animator.SetTrigger("MeleeCombo");
        }
    
        else if (randomNumber == 1)
        {
            rotateSpeed = 100;
            animator.SetTrigger("DownAttack");
        }
            
       
    }


    private void ResetAttack()
    {
        StartCoroutine(CanChase(2f));
        Debug.Log("Resetting");
        canRotate = true;
    }

    private IEnumerator CanChase(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
        Debug.Log("Reset");
    }

    private void EnableHit()
    {
        canHit = true;
        checkHitbox = true;
        if (randomNumber == 1) 
            canRotate = false;

    }

    private void DisableHit()
    {
        canHit = false;
        checkHitbox = false;
        rotateSpeed = 5f;
    }

  

   


}
