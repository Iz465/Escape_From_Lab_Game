using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float damage;
    public float moveSpeed;
    public float atkSpeed;
    [HideInInspector]
    public float health;
    public float maxHealth;

    public void ResetStats()
    {
        health = maxHealth;
    }

   
}
