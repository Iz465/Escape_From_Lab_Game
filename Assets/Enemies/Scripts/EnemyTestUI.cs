using UnityEngine;
using UnityEngine.UI;


public class EnemyTestUI : MonoBehaviour
{
    [SerializeField]
    private Text health;
    [SerializeField]
    private TestEnemy enemy;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!health) return;
        if (!enemy) return;
        health.text = $"HEALTH : {enemy.health}";
    }

}
