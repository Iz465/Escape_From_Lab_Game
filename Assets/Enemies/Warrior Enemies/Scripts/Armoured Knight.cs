using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class ArmouredKnight : navmeshtestscript
{
    private bool canDamage = true;
 

    [SerializeField] private GameObject Armour;
    [SerializeField] private ParticleSystem armourParticle;
    public Color[] colours = new Color[3] { Color.red, Color.green, Color.blue };

    [SerializeField] private AudioClip swordHitSound;
    private int oldNumber;
    private int[] storedNumber = new int[1];
    private bool doOnce = true;

    [SerializeField] private ParticleSystem swordParticle;
    [SerializeField] private Transform particleAttackLocation;
    [SerializeField] private Transform swordLocation;
    private IEnumerator ResetAnim(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    bool doOnce1 = false;
    bool doOnce2 = false;

    public override void TakeDamage(float damageTaken)
    {
     

      


        if (canDamage)
            base.TakeDamage(damageTaken);

        if (health <= 360 && health > 220 && !doOnce1) { ActivateArmour(); doOnce1 = true; health = 220; return; }
        if (health <= 80 && health > 0 && !doOnce2) { ActivateArmour(); doOnce2 = true; health = 80; return; }

    }

    public int storedArmourColour;
    private Coroutine activateExplosion;
    private void ActivateArmour()
    {

        animator.SetBool("ArmourActivated", true);    // cancels the enemy attack so that the next phase can start
        canMove = false;
        activateExplosion = StartCoroutine(ActivateExplosion(1.5f)); // how long the player has to destroy the orb

        Debug.Log("IMMUNE");
        playerCanDash = false;

        Move move = player.GetComponent<Move>();
        if (move) 
        { 
            Talon_Rhyke talon_Rhyke = player.GetComponent<Talon_Rhyke>();
            if (talon_Rhyke)
            {
                Debug.Log("SLOWING FALL");
                move.fallSpeed = 0;
                move.fallAcceleration = 0.005f; StartCoroutine(talon_Rhyke.ResetFallAcceleration(move, 3));
            }
     
        }

        storedArmourColour = Random.Range(0, colours.Length);
        armourParticle.gameObject.SetActive(true);
        var main = armourParticle.main;
        colours[storedArmourColour].a = 0.3f;
        main.startColor = colours[storedArmourColour];
        colours[storedArmourColour].a = 1f;



        // this will make player go a certain distance away from the enemies direction.
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller) controller.enabled = false;
        Animator playerAnimator = player.GetComponentInChildren<Animator>();
        playerAnimator.SetBool("DisablePlayer", true);
        playerAnimator.Play("Idle", 0, 0f);
        ArcSwing swing = player.GetComponentInChildren<ArcSwing>();
        if (swing) swing.attackDisabled = true;
  

        else Debug.Log("NO SWING ON PLAYER");
        StartCoroutine(ReactivateMovement(controller, playerAnimator, 0.5f));




        canDamage = false;
    

        Vector3[] orbLocations =
    {
        new Vector3(-10, 10, 0),   
        new Vector3(0, 10, 0),   
        new Vector3(10, 10, 0)     
    };


      
        List<Color> colourList = new List<Color>(colours);

        for (int i = 0; i < 3; i ++)
        {
            // use transform point to switch the global transform to local. use it whenever you want stuff to spawn in same position no matter the parents rotation
            Vector3 orbPosition = transform.TransformPoint(orbLocations[i]);

            GameObject armourInstance = Instantiate(Armour, orbPosition, Quaternion.identity);
            ArmourOrb orb = armourInstance.GetComponent<ArmourOrb>();

            // gets it from the colour list count, which does not change.
            int orbColour = Random.Range(0, colourList.Count);

            // gets the colour instead of the index number
            Color chosenColour = colourList[orbColour];

            // gets the index that this colour is for the original list.
            int colourIndex = System.Array.IndexOf(colours, chosenColour);

            foreach (ParticleSystem particle in armourInstance.GetComponentsInChildren<ParticleSystem>())
            {
             

                // different colour each time
                
                var childMain = particle.main;
                childMain.startColor = chosenColour;


              
            }

            // it stores the index of the original list instead of the number which will remove colour numbers from appearing after theyve been used.
            orb.colourNumber = colourIndex;
            // removes colour from list so there cant be duplicates
            colourList.RemoveAt(orbColour);
            if (!orb) return;
            orb.Knight = gameObject;

        }
    
   
    }


    public void DeactivateArmour()
    {
        if (activateExplosion != null) StopCoroutine(activateExplosion);
        DestroyOrbs();
      
        // this will make player go a certain distance away from the enemies direction.
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller) controller.enabled = false;
        Animator playerAnimator = player.GetComponentInChildren<Animator>();
        playerAnimator.SetBool("DisablePlayer", true);
        ArcSwing swing = player.GetComponentInChildren<ArcSwing>();
        if (swing) swing.attackDisabled = true;


        else Debug.Log("NO SWING ON PLAYER");
        StartCoroutine(ReactivateMovement(controller, playerAnimator, 0.5f));

        canDamage = true;
        armourParticle.gameObject.SetActive(false);
        animator.SetBool("ArmourActivated", false);
    }

    private IEnumerator ReactivateMovement(CharacterController controller, Animator playerAnimator, float timer)
    {

        float time = 0;

        Vector3 playerDirection = (player.transform.position - gameObject.transform.position);
        playerDirection.y = 0;
        if (playerDirection.sqrMagnitude < 0.01)
            playerDirection = Vector3.forward;
        playerDirection.Normalize();

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = gameObject.transform.position + (playerDirection * 20);

        while (time < timer)
        {
            player.transform.position = Vector3.Lerp(startPosition, endPosition, time / timer);
            time += Time.deltaTime;
            yield return null;
        }

        player.transform.position = endPosition;
        controller.enabled = true;
        playerAnimator.SetBool("DisablePlayer", false);
        ArcSwing swing = player.GetComponentInChildren<ArcSwing>();
        if (swing) swing.attackDisabled = false;

    }


    private IEnumerator ActivateExplosion(float time)
    {
        yield return new WaitForSeconds(time);
        player.TakeDamage(35);
        armourParticle.gameObject.SetActive(false);
        DestroyOrbs();
        playerCanDash = true;
        canDamage = true;
        animator.SetBool("ArmourActivated", false);
      
    }

    public void DestroyOrbs()
    {
        // using this as its not deprecated. Use this whenever i want all the objects of a certain type stored.
        ArmourOrb[] foundOrbs = FindObjectsByType<ArmourOrb>(FindObjectsSortMode.None);
        foreach (ArmourOrb foundOrb in foundOrbs)
        {
            Instantiate(player.playerHitParticle, foundOrb.transform.position, Quaternion.identity);
            Destroy(foundOrb.gameObject);
        }

        canMove = true;

    }


    private void FirstKnightStep()
    {
      //  globalEnemyManager.CheckEnemySound(firstStepSound, "footsteps", audioSource);
    }

    private void SecondKnightStep()
    {
     //   globalEnemyManager.CheckEnemySound(secondStepSound, "footsteps", audioSource);
    }

    // The window during the enemies attack animation that allows them to damage the player.
    private void EnableHit()
    {
     
    }

    private void DamagePlayer()
    {
      //  Debug.Log($"stored number at damage player function: {storedNumber[0]}");
        audioSource.PlayOneShot(swordHitSound, 3f);
        switch (storedNumber[0])
        {
            case 0: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(25); break;
            case 1: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(25); break;
            case 2: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.blue) player.TakeDamage(25); break;
        }
    }

    private void SwitchAttackType()
    {
        oldNumber = storedNumber[0];

        while (oldNumber == storedNumber[0])
        {
            int randomNumber = Random.Range(0, 3);
            storedNumber[0] = randomNumber;
        }

        // Added this as it doesnt work on the first time.
        if (doOnce)
        {
            SelectColour();

            oldNumber = storedNumber[0];
            doOnce = false;
        }

        SelectColour();
    }

    private void SelectColour()
    {
        ParticleSystem swordInstance = Instantiate(swordParticle, swordLocation);
        audioSource.PlayOneShot(attackSound);
  //      Debug.Log($"Select colour function stored number: {storedNumber[0]}");
        switch (storedNumber[0])
        {
            case 0: LoopChildren(swordInstance, Color.red); break;
            case 1: LoopChildren(swordInstance, Color.green); break;
            case 2: LoopChildren(swordInstance, Color.blue); break;
        }

    }

    private void LoopChildren(ParticleSystem swordInstance, Color colour)
    {
        foreach (ParticleSystem child in swordInstance.GetComponentsInChildren<ParticleSystem>())
        {
            var main = child.main;
            main.startColor = colour;
        }
    }

    private void DisableHit()
    {
   


    }


    // Decides whether to attack or chase depending on where player is
    private void CheckPlayerRange()
    {

       

    }

    protected override void Update()
    {
        animator.SetFloat("PlayerDistance", distanceToPlayer);
        bool inRange = distanceToPlayer <= 6f;
        animator.SetBool("ShouldAttack", inRange);
        canAttack = true;
       
        
     


        base.Update();
    }

 
}
