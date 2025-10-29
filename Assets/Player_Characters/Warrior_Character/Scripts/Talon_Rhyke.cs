using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Talon_Rhyke : Player
{

    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask wallLayer;


    protected override void Update()
    {
        base.Update();

        if (MeleeHitDetection.canTrigger)
            CheckEnemyHit();

    }



    private void CheckEnemyHit()
    {
        RaycastHit hit;
        bool checkPlayer = Physics.Linecast(point1.position, point2.position, out hit, enemyLayer);

        if (checkPlayer)
        {
            navmeshtestscript enemy = hit.collider.gameObject.GetComponent<navmeshtestscript>();
            if (!enemy)
                enemy = hit.collider.gameObject.GetComponentInParent<navmeshtestscript>();
            enemy.TakeDamage(20);


            if (!enemy.canHitMultiple || MeleeHitDetection.enemiesHit.Contains(enemy))
            {
                MeleeHitDetection.canTrigger = false;
                return;
            }

            MeleeHitDetection.enemiesHit.Add(enemy);





        }

        bool checkWall = Physics.Linecast(point1.position, point2.position, out hit, wallLayer);

        if (checkWall && BreakableWall.canHitWall)
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

    }

}
