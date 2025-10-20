using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageTaken
{
    [System.Serializable]
    public struct PlayerStats
    {
        public string name;
        public float health;
        public float stamina;
    }
    public PlayerStats stats;
    [HideInInspector]
    public float maxHealth;
    [HideInInspector]
    public float maxStamina;
    public static bool canDamage = true;
    [HideInInspector] public ParticleSystem playerHitParticle;
    private void Awake()
    {
        maxHealth = stats.health;
        maxStamina = stats.stamina;
    }

    virtual protected void Update()
    {
        stats.stamina += 5f * Time.deltaTime;
        stats.stamina = Mathf.Clamp(stats.stamina, 0, maxStamina);

        if (stats.health <= 0)
            playerDeath();
    }


    public void TakeDamage(float damageTaken) 
    {
        if (!canDamage) return;
        stats.health -= damageTaken;
        if (stats.health <= 0)
            playerDeath();
    }


    public void playerDeath() 
    {
        Debug.Log("You have died");
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        Debug.Log($"Scene name : {scene.name}");
        SceneManager.LoadScene(scene.name);

       
       
       // gameObject.SetActive(false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
     //   Debug.Log($"Controller hit something : {hit.gameObject}");
    }



}
