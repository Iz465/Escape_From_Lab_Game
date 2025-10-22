using UnityEngine;

public class Talon_Rhyke : Player
{
    private Move move;
    private Animator animator;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private LayerMask enemyLayer;

    private void Start()
    {
        move = GetComponent<Move>();
        animator = GetComponentInChildren<Animator>();
    }
    protected override void Update()
    {
        base.Update();
        if (!animator)
        {
            Debug.Log("No Animator");
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pausing for debug!");
            Debug.Break();
        }


        Vector3 movement = move.controller.velocity;

        // Ignores jumping/falling
        Vector3 horizontalVelocity = new Vector3(movement.x, 0, movement.z);


        if (horizontalVelocity.magnitude > 0.1f)
            animator.SetBool("Moving", true);

        else
            animator.SetBool("Moving", false);


        if (MeleeHitDetection.canTrigger)
            CheckEnemyHit();

    


    }



    private void CheckEnemyHit()
    {
        RaycastHit hit; 
        bool checkPlayer = Physics.Linecast(point1.position, point2.position, out hit,enemyLayer);

        if (checkPlayer)
        {
            navmeshtestscript enemy = hit.collider.gameObject.GetComponent<navmeshtestscript>();
            if (!enemy)
                enemy = hit.collider.gameObject.GetComponentInParent<navmeshtestscript>();
            enemy.TakeDamage(20);
            MeleeHitDetection.canTrigger = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point1.position, point2.position);
 
    }

    }
