using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using System.Collections;
using UnityEngine;

public class Talon_Rhyke : Player
{

    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
    [SerializeField] private Transform point4;
    [SerializeField] private Transform point5;
    [SerializeField] private Transform point6;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask pillarLayer;
    [SerializeField] public AudioClip hammerSwingSound;

    private Coroutine resetCoroutine;

    protected override void Update()
    {
        base.Update();

        if (MeleeHitDetection.canTrigger)
            CheckEnemyHit();

    }

 


    private void CheckEnemyHit()
    {
        RaycastHit hit;
        bool canDamage = false;
        RaycastHit storedHit = default;
        bool checkEnemy = Physics.Linecast(point1.position, point2.position, out hit, enemyLayer);
        if (checkEnemy && !canDamage)
        {
            storedHit = hit;
            canDamage = true;
        }

            bool checkEnemy2 = Physics.Linecast(point3.position, point4.position, out hit, enemyLayer);
        if (checkEnemy2 && !canDamage)
        {
            storedHit = hit;
            canDamage = true;
        }


        bool checkEnemy3 = Physics.Linecast(point5.position, point6.position, out hit, enemyLayer);
        if (checkEnemy3 && !canDamage)
        {
            storedHit = hit;
            canDamage = true;
        }


            if (canDamage)
        {
            navmeshtestscript enemy = storedHit.collider.gameObject.GetComponent<navmeshtestscript>();
            if (!enemy)
                enemy = storedHit.collider.gameObject.GetComponentInParent<navmeshtestscript>();
            enemy.TakeDamage(20);


            if (!enemy.canHitMultiple || MeleeHitDetection.enemiesHit.Contains(enemy))
            {
                MeleeHitDetection.canTrigger = false;
                return; 
            }

            MeleeHitDetection.enemiesHit.Add(enemy);

        }


        bool checkWall = Physics.Linecast(point1.position, point2.position, out hit, wallLayer);
        bool checkWall2 = Physics.Linecast(point3.position, point4.position, out hit, wallLayer);
        bool checkWall3 = Physics.Linecast(point5.position, point6.position, out hit, wallLayer);

        if (checkWall && BreakableWall.canHitWall || checkWall2 && BreakableWall.canHitWall || checkWall3 && BreakableWall.canHitWall)
        {


            BreakableWall wall = hit.collider.gameObject.GetComponent<BreakableWall>();
            if (!wall) return;

            
            Instantiate(playerHitParticle, hit.point, Quaternion.identity);
            wall.WallDamage(20);
            BreakableWall.canHitWall = false;
        }

        bool checkPillar = Physics.Linecast(point1.position, point2.position, out hit, pillarLayer);
        bool checkPillar2 = Physics.Linecast(point3.position, point4.position, out hit, pillarLayer);
        bool checkPillar3 = Physics.Linecast(point5.position, point6.position, out hit, pillarLayer);

        // line stops the annoying errors coming up
        if (!hit.collider) return;
        if (checkPillar && ((1 << hit.collider.gameObject.layer) & pillarLayer) != 0 || checkPillar2 && ((1 << hit.collider.gameObject.layer) & pillarLayer) != 0 || checkPillar3 && ((1 << hit.collider.gameObject.layer) & pillarLayer) != 0)
        {
            Instantiate(playerHitParticle, hit.point, Quaternion.identity);

            ArmourOrb orb = hit.collider.gameObject.GetComponent<ArmourOrb>();
            if (orb)
            {
                ArmouredKnight armouredKnight = orb.Knight.GetComponent<ArmouredKnight>();
                Move move = gameObject.GetComponent<Move>();
                if (!move) return;

                move.fallSpeed = 0;
                move.fallAcceleration = 0.005f;
                if (resetCoroutine != null) StopCoroutine(resetCoroutine);
      
                resetCoroutine = StartCoroutine(ResetFallAcceleration(move, .3f));

                if (armouredKnight)
                {

                    armouredKnight.playerCanDash = true;
                    armouredKnight.DeactivateArmour();

                    Debug.Log($"Orb colour: {orb.colourNumber}");
                    Debug.Log($"Armour colour: {armouredKnight.storedArmourColour}");

                    if (armouredKnight.storedArmourColour != orb.colourNumber)
                    {
                        TakeDamage(35);
                    }

                }

                Destroy(hit.collider.gameObject);
            }

            else
            {
                HittablePillar pillarHit = hit.collider.gameObject.GetComponent<HittablePillar>();
                if (pillarHit)
                    StartCoroutine(pillarHit.DisablePillar(2.5f));
            }
              

          

        }
           

        
    }

    public IEnumerator ResetFallAcceleration(Move move, float time)
    {
        yield return new WaitForSeconds(time);
        move.fallAcceleration = 2f;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point1.position, point2.position);
        Gizmos.DrawLine(point3.position, point4.position);
        Gizmos.DrawLine(point5.position, point6.position);

    }

}
