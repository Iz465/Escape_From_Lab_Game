using System.Collections.Generic;
using Unity.MLAgents.Sensors;
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
    [SerializeField] public AudioClip hammerSwingSound;



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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point1.position, point2.position);
        Gizmos.DrawLine(point3.position, point4.position);
        Gizmos.DrawLine(point5.position, point6.position);

    }

}
