using UnityEngine;

public class Kael_Draven : Player
{
    Move move;
    Animator animator;
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

     
        if (horizontalVelocity.magnitude > 0.1f ) 
            animator.SetBool("Moving", true);
        
        else
            animator.SetBool("Moving", false);
        
    }

  


}
